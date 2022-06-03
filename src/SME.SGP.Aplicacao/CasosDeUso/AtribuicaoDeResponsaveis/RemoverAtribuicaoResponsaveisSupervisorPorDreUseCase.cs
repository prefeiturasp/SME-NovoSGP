using MediatR;
using SME.SGP.Aplicacao.Commands.AtribuicaoDeResponsaveis.RemoverAtribuicaoSupervisor;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.AtribuicaoDeResponsaveis.ObterSupervisoresPorDreQuery;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoResponsaveisSupervisorPorDreUseCase : IRemoverAtribuicaoResponsaveisSupervisorPorDreUseCase
    {
        private readonly IMediator mediator;
        public RemoverAtribuicaoResponsaveisSupervisorPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var codigoDre = param.ObterObjetoMensagem<string>();
                var supervisoresEscolasDres = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(codigoDre, TipoResponsavelAtribuicao.SupervisorEscolar));

                if (supervisoresEscolasDres.Any())
                {
                    var supervisoresIds = supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                    var supervisores = await mediator.Send(new ObterSupervisorPorCodigoQuery(supervisoresIds.ToArray()));

                    if (supervisores != null && supervisores.Any())
                    {
                        if (supervisores.Count() != supervisoresIds.Count())
                            RemoverSupervisorSemAtribuicao(supervisoresEscolasDres, supervisores);
                    }
                }
                else { return false; }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel Supervisor por DRE", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private void RemoverSupervisorSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, IEnumerable<SupervisoresRetornoDto> supervisoresEol)
        {
            var supervisoresSemAtribuicao = supervisoresEscolasDres;

            if (supervisoresEol != null)
            {
                supervisoresSemAtribuicao = supervisoresEscolasDres
                    .Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.SupervisorEscolar &&
                        !supervisoresEol.Select(e => e.CodigoRf)
                    .Contains(s.SupervisorId));
            }

            if (supervisoresSemAtribuicao != null && supervisoresSemAtribuicao.Any())
            {
                foreach (var supervisor in supervisoresSemAtribuicao)
                {
                    if (supervisor.Tipo == (int)TipoResponsavelAtribuicao.SupervisorEscolar)
                    {
                        var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                        mediator.Send(new RemoverAtribuicaoSupervisorCommand(supervisorEntidadeExclusao));
                    }
                }
            }
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