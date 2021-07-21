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


            var totalCompensacoes = await mediator.Send(new ObterTotalCompensacaoAusenciaPorAnoLetivoQuery(anoLetivo, dreId, ueId, modalidade, semestre, bimestre));
            

            return MapearParaDto(dadosAusenciaAlunos, totalCompensacoes.TotalCompensacoesFormatado);
        }

        private GraficoCompensacaoAusenciaDto MapearParaDto(IEnumerable<TotalCompensacaoAusenciaDto> dadosAusenciaAlunos, string tagTotalAusencia)
        {
            var dadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>();

            foreach (var compensacaoAusencia in dadosAusenciaAlunos)
            {
                dadosCompensacaoAusenciaDashboard.Add(new DadosRetornoAusenciasCompensadasDashboardDto()
                {
                    Descricao = compensacaoAusencia.DescricaoAnoTurmaFormatado,
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
