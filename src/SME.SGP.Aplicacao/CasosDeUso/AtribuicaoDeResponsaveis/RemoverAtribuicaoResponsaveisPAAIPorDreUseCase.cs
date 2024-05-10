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

                var reponsaveisPAAI = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(codigoDre, TipoResponsavelAtribuicao.PAAI));

                if (reponsaveisPAAI.Any())
                {
                    var responsaveisEOL = await mediator.Send(new ObterFuncionariosPorPerfilDreQuery(Perfis.PERFIL_PAAI, codigoDre));

                    return await RemoverPAAISemAtribuicao(reponsaveisPAAI, responsaveisEOL);
                }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel PAAI por DRE", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private async Task<bool> RemoverPAAISemAtribuicao(IEnumerable<SupervisorEscolasDreDto> reponsaveisPAAI, IEnumerable<UsuarioEolRetornoDto> responsaveisEOL)
        {

            if (responsaveisEOL.NaoEhNulo())
                reponsaveisPAAI = reponsaveisPAAI.Where(s => s.TipoAtribuicao == (int)TipoResponsavelAtribuicao.PAAI && !responsaveisEOL.Select(e => e.CodigoRf).Contains(s.SupervisorId));

            foreach (var supervisor in reponsaveisPAAI)
            {
                var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                await mediator.Send(new RemoverAtribuicaoSupervisorCommand(supervisorEntidadeExclusao));
            }
            return true;
        }
        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.AtribuicaoSupervisorId,
                Excluido = dto.AtribuicaoExcluida,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF,
                Tipo = dto.TipoAtribuicao
            };
        }
    }
}
