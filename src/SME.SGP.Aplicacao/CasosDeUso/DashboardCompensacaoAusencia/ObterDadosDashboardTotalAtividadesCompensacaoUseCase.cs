using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTotalAtividadesCompensacaoUseCase : AbstractUseCase, IObterDadosDashboardTotalAtividadesCompensacaoUseCase
    {
        public ObterDadosDashboardTotalAtividadesCompensacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<GraficoCompensacaoAusenciaDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre)
        {
            var dadosAtividadesCompensadas = await mediator.Send(new ObterDadosDashboardTotalAtividadesCompensacaoQuery(anoLetivo,
                                                                                                          dreId,
                                                                                                          ueId,
                                                                                                          modalidade,
                                                                                                          bimestre,
                                                                                                          semestre));

            return MapearParaDto(dadosAtividadesCompensadas, "");
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
