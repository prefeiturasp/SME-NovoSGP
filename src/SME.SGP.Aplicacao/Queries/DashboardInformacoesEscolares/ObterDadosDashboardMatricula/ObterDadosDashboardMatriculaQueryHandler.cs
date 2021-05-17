using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardMatriculaQueryHandler : IRequestHandler<ObterDadosDashboardMatriculaQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma;

        public ObterDadosDashboardMatriculaQueryHandler(IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma)
        {
            this.repositorioConsolidacaoMatriculaTurma = repositorioConsolidacaoMatriculaTurma ?? throw new System.ArgumentNullException(nameof(repositorioConsolidacaoMatriculaTurma));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterDadosDashboardMatriculaQuery request, CancellationToken cancellationToken)
        {
            var dadosGrafico = await repositorioConsolidacaoMatriculaTurma.ObterGraficoMatriculasAsync(request.AnoLetivo, request.DreId, request.UeId, request.Ano, request.Modalidade, request.Semestre);
            return MontarDto(dadosGrafico, request.DreId);
        }

        private IEnumerable<GraficoBaseDto> MontarDto(IEnumerable<TotalMatriculaPorDreEAnoDto> totalMatriculaPorDreEAnoDtos, long dreId)
        {
            var listaGraficos = new List<GraficoBaseDto>();
            if (totalMatriculaPorDreEAnoDtos.Any())
            {
                foreach (var total in totalMatriculaPorDreEAnoDtos)
                {
                    var grafico = new GraficoBaseDto()
                    {
                        Descricao = dreId > 0 ? total.AnoDescricao : FormatarAbreviacaoDre(total.DreNome),
                        Quantidade = total.Quantidade
                    };
                    listaGraficos.Add(grafico);
                }
            }
            return listaGraficos;
        }

        private static string FormatarAbreviacaoDre(string abreviacaoDre)
            => abreviacaoDre.Replace(DashboardFrequenciaConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();
    }
}
