using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase : AbstractUseCase, IExecutarBuscarConsolidadoAtendimentosProfissionalAtendimentoNAAPAUseCase
    {
        public ExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroBuscarAtendimentosProfissionalConsolidadoAtendimentoNAAPADto>();
            var atendimentosProfissional = await mediator.Send(new ObterAtendimentosProfissionalEncaminhamentosNAAPAConsolidadoCargaQuery(filtro.UeId, filtro.Mes, filtro.AnoLetivo));
            foreach (var profissional in atendimentosProfissional)
            {
                var entidade = new ConsolidadoAtendimentoNAAPA
                {
                    UeId = profissional.UeId,
                    AnoLetivo = profissional.AnoLetivo,
                    Quantidade = profissional.Quantidade,
                    Mes = profissional.Mes,
                    NomeProfissional = profissional.NomeProfissional,
                    RfProfissional = profissional.RfProfissional,
                    Modalidade = profissional.Modalidade
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA, entidade, Guid.NewGuid()));
            }
            await PublicarExclusaoConsolidacao(filtro.UeId, filtro.Mes, filtro.AnoLetivo, atendimentosProfissional);
            return true;
        }

        private async Task PublicarExclusaoConsolidacao(long ueId, int mes, int anoLetivo, IEnumerable<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto> atendimentosProfissionalConsolidacao)
        {
            var profissionaisConsolidacao = atendimentosProfissionalConsolidacao.Select(p => p.RfProfissional).Distinct();
            var param = new FiltroExcluirAtendimentosProfissionalConsolidadoAtendimentoNAAPADto(ueId, mes, anoLetivo, profissionaisConsolidacao.ToArray());
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA, param, Guid.NewGuid()));
        }
    }
}