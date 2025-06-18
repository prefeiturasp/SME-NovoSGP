using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoResponsaveisPAAIPorDreUseCase : IRemoverAtribuicaoResponsaveisPAAIPorDreUseCase
    {
        private readonly IMediator mediator;
        public RemoverAtribuicaoResponsaveisPAAIPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var codigoDre = param.ObterObjetoMensagem<string>();
                if (string.IsNullOrEmpty(codigoDre))
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível obter o código da DRE da mensagem.", LogNivel.Critico, LogContexto.AtribuicaoReponsavel));
                    return false;
                }

                // ETAPA 1: Obter dados de ambas as fontes.
                var atribuicoesSgp = await ObterAtribuicoesAtuaisSgp(codigoDre);
                if (!atribuicoesSgp.Any())
                    return true;

                var responsaveisAtivosEol = await ObterResponsaveisAtivosEol(codigoDre);

                // ETAPA 2: Identificar quais atribuições devem ser removidas.
                var atribuicoesIdsParaRemover = FiltrarAtribuicoesParaRemocao(atribuicoesSgp, responsaveisAtivosEol);
                if (!atribuicoesIdsParaRemover.Any())
                    return true; // Processo concluído com sucesso, sem divergências.

                // ETAPA 3: Executar a remoção em lote.
                await RemoverAtribuicoesEmLote(atribuicoesIdsParaRemover);

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel PAAI por DRE", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private Task<IEnumerable<SupervisorEscolasDreDto>> ObterAtribuicoesAtuaisSgp(string codigoDre)
        {
            return mediator.Send(new ObterSupervisoresPorDreAsyncQuery(codigoDre, TipoResponsavelAtribuicao.PAAI));
        }

        private Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveisAtivosEol(string codigoDre)
        {
            return mediator.Send(new ObterFuncionariosPorPerfilDreQuery(Perfis.PERFIL_PAAI, codigoDre));
        }

        private IEnumerable<long> FiltrarAtribuicoesParaRemocao(
            IEnumerable<SupervisorEscolasDreDto> atribuicoesSgp,
            IEnumerable<UsuarioEolRetornoDto> responsaveisAtivosEol)
        {
            // Se a lista de responsáveis do EOL for nula ou vazia, é mais seguro não remover ninguém
            // e registrar um log, pois pode indicar uma falha na consulta de origem.
            if (responsaveisAtivosEol == null || !responsaveisAtivosEol.Any())
            {

                throw new Exception("A consulta de responsáveis PAAI no EOL retornou vazia.");
            }

            var rfResponsaveisAtivos = responsaveisAtivosEol.Select(e => e.CodigoRf).ToHashSet();

            return atribuicoesSgp
                .Where(atribuicao => !rfResponsaveisAtivos.Contains(atribuicao.SupervisorId))
                .Select(atribuicao => atribuicao.AtribuicaoSupervisorId);
        }

        private Task RemoverAtribuicoesEmLote(IEnumerable<long> atribuicoesIdsParaRemover)
        {
            // Isso é muito mais performático do que enviar um comando por item.
            var command = new RemoverAtribuicoesResponsaveisCommand(atribuicoesIdsParaRemover);
            return mediator.Send(command);
        }
    }
}
