using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTotalAusenciasCompensadasUseCase : AbstractUseCase, IObterDadosDashboardTotalAusenciasCompensadasUseCase
    {
        public ObterDadosDashboardTotalAusenciasCompensadasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<GraficoCompensacaoAusenciaDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre)
        {
            var dadosAusenciaAlunos = await mediator.Send(new ObterDadosDashboardTotalAusenciasCompensadasQuery(anoLetivo,
                                                                                                          dreId,
                                                                                                          ueId,
                                                                                                          modalidade,
                                                                                                          bimestre,
                                                                                                          semestre));

            // obter total compessação ausência

            return MapearParaDto(dadosAusenciaAlunos, "");
        }

        private GraficoCompensacaoAusenciaDto MapearParaDto(IEnumerable<TotalAusenciasCompensadasDto> dadosAusenciaAlunos, string tagTotalAusencia)
        {
            var dadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>();

            foreach (var compensacaoAusencia in dadosAusenciaAlunos)
            {
                dadosCompensacaoAusenciaDashboard.Add(new DadosRetornoAusenciasCompensadasDashboardDto()
                {
                    Descricao = compensacaoAusencia.DescricaoAnoTurma,
                    Quantidade = compensacaoAusencia.Quantidade
                });
            }

            return new GraficoCompensacaoAusenciaDto()
            {
                TagTotalCompensacaoAusencia = tagTotalAusencia,
                DadosCompensacaoAusenciaDashboard = dadosCompensacaoAusenciaDashboard
            };
        }
    }
}
