using MediatR;
using SME.SGP.Dominio;
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
            var dadosGrafico = await repositorioConsolidacaoMatriculaTurma.ObterGraficoMatriculasAsync(request.AnoLetivo, request.DreId, request.UeId, request.Anos, request.Modalidade, request.Semestre);
            return MontarDto(dadosGrafico, request.DreId, request.UeId, request.Anos);
        }

        private IEnumerable<GraficoBaseDto> MontarDto(IEnumerable<InformacoesEscolaresPorDreEAnoDto> totalMatriculaPorDreEAnoDtos, long dreId, long ueId, AnoItinerarioPrograma[] anos)
        {
            var listaGraficos = new List<GraficoBaseDto>();
            if (totalMatriculaPorDreEAnoDtos.Any())
            {
                foreach (var total in totalMatriculaPorDreEAnoDtos)
                {
                    var descricao = "";
                    if (ueId > 0 && anos != null && anos.Count() == 1)
                        descricao = total.TurmaDescricao;
                    else if (dreId > 0)
                        descricao = total.AnoDescricao;
                    else descricao = FormatarAbreviacaoDre(total.DreDescricao);
                    var grafico = new GraficoBaseDto()
                    {
                        Descricao = descricao,
                        Quantidade = total.Quantidade
                    };
                    listaGraficos.Add(grafico);
                }
            }
            return listaGraficos;
        }

        private static string FormatarAbreviacaoDre(string abreviacaoDre)
            => abreviacaoDre.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();
    }
}
