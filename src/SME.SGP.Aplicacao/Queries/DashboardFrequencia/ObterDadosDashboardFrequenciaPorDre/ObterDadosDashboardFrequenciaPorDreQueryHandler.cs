using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorDreQueryHandler : IRequestHandler<ObterDadosDashboardFrequenciaPorDreQuery, IEnumerable<GraficoFrequenciaGlobalPorDREDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioConsolidacaoFrequenciaTurma;

        public ObterDadosDashboardFrequenciaPorDreQueryHandler(IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma;
        }

        public async Task<IEnumerable<GraficoFrequenciaGlobalPorDREDto>> Handle(ObterDadosDashboardFrequenciaPorDreQuery request, CancellationToken cancellationToken)
        {
            var resultado = new List<GraficoFrequenciaGlobalPorDREDto>();

            var frequenciasGlobaisPorDre = await repositorioConsolidacaoFrequenciaTurma.ObterFrequenciaGlobalPorDreAsync(request.AnoLetivo, request.Modalidade, request.Ano, request.Semestre);
            if (!frequenciasGlobaisPorDre?.Any() ?? true)
                return resultado;

            if (frequenciasGlobaisPorDre != null)
            {
                foreach (var frequenciaGlobalPorDre in frequenciasGlobaisPorDre)
                {
                    var graficoFrequenciaAcimaDoMinimo = new GraficoFrequenciaGlobalPorDREDto
                    {
                        Dre = FormatarAbreviacaoDre(frequenciaGlobalPorDre.Dre),
                        Descricao = DashboardConstants.QuantidadeAcimaMinimoFrequenciaDescricao,
                        Quantidade = frequenciaGlobalPorDre.QuantidadeAcimaMinimoFrequencia
                    };

                    var graficoFrequenciaAbaixoDoMinimo = new GraficoFrequenciaGlobalPorDREDto
                    {
                        Dre = FormatarAbreviacaoDre(frequenciaGlobalPorDre.Dre),
                        Descricao = DashboardConstants.QuantidadeAbaixoMinimoFrequenciaDescricao,
                        Quantidade = frequenciaGlobalPorDre.QuantidadeAbaixoMinimoFrequencia
                    };

                    resultado.Add(graficoFrequenciaAcimaDoMinimo);
                    resultado.Add(graficoFrequenciaAbaixoDoMinimo);
                }
            }

            return resultado;
        }

        private static string FormatarAbreviacaoDre(string abreviacaoDre)
            => abreviacaoDre.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();
    }
}