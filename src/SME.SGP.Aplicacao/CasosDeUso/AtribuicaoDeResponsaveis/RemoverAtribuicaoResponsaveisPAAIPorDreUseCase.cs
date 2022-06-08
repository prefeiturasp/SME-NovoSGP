using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                var responsavelPAAI = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(codigoDre, TipoResponsavelAtribuicao.PAAI));

                if (responsavelPAAI.Any())
                {
                    var funcionariosEOL = await mediator.Send(new ObterFuncionariosPorPerfilDreQuery(Perfis.PERFIL_PAAI, codigoDre));

                    if (funcionariosEOL != null && funcionariosEOL.Any())
                    {
                        return await RemoverPAAISemAtribuicao(responsavelPAAI, funcionariosEOL);
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel PAAI por DRE", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private async Task<bool> RemoverPAAISemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, IEnumerable<UsuarioEolRetornoDto> supervisoresEol)
        {

            if (supervisoresEol != null)
            {
                supervisoresEscolasDres = supervisoresEscolasDres
                    .Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.PAAI &&
                        !supervisoresEol.Select(e => e.CodigoRf)
                    .Contains(s.SupervisorId));
            }

            if (supervisoresEscolasDres != null && supervisoresEscolasDres.Any())
            {
                foreach (var supervisor in supervisoresEscolasDres)
                {
                    var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                    await mediator.Send(new RemoverAtribuicaoSupervisorCommand(supervisorEntidadeExclusao));
                }
                return true;
            }
            await mediator.Send(new SalvarLogViaRabbitCommand("Não foram encontrados responsáveis para atribuição no EOL", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, "Responsável PAAI"));
            return false;
        }
        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.Id,
                Excluido = dto.Excluido,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF,
                Tipo = dto.Tipo
            };
        }
    }
}
