using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.DashboardDevolutivas.ObterDevolutivasEstimadasEConfirmadas
{
    public class ObterDevolutivasEstimadasEConfirmadasQueryHandler : IRequestHandler<ObterDevolutivasEstimadasEConfirmadasQuery, IEnumerable<GraficoDevolutivasEstimadasEConfirmadasDto>>
    {
        private readonly IRepositorioConsolidacaoDevolutivasConsulta repositorioConsolidacaoDevolutivas;

        public ObterDevolutivasEstimadasEConfirmadasQueryHandler(IRepositorioConsolidacaoDevolutivasConsulta repositorioConsolidacaoDevolutivas)
        {
            this.repositorioConsolidacaoDevolutivas = repositorioConsolidacaoDevolutivas;
        }

        public async Task<IEnumerable<GraficoDevolutivasEstimadasEConfirmadasDto>> Handle(ObterDevolutivasEstimadasEConfirmadasQuery request, CancellationToken cancellationToken)
        {
            var resultado = new List<GraficoDevolutivasEstimadasEConfirmadasDto>();

            var consolidacaoDeDevolutivas = await repositorioConsolidacaoDevolutivas.ObterDevolutivasEstimadasEConfirmadasAsync(request.AnoLetivo, request.Modalidade, request.DreId, request.UeId);
            if (!consolidacaoDeDevolutivas?.Any() ?? true) return resultado;

            foreach (var consolidacaoDeDevolutiva in consolidacaoDeDevolutivas)
            {
                var devolutivasEstimadas = new GraficoDevolutivasEstimadasEConfirmadasDto
                {
                    Descricao = DashboardDevolutivasConstants.QuantidadeDeDevolutivasEstimadas,
                    Quantidade = consolidacaoDeDevolutiva.DevolutivasEstimadas,
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId.HasValue, consolidacaoDeDevolutiva.TurmaAno, request.Modalidade)
                };

                var devolutivasRegistradas = new GraficoDevolutivasEstimadasEConfirmadasDto
                {
                    Descricao = DashboardDevolutivasConstants.QuantidadeDeDevolutivasRegistradas,
                    Quantidade = consolidacaoDeDevolutiva.DevolutivasRegistradas,
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId.HasValue, consolidacaoDeDevolutiva.TurmaAno, request.Modalidade)
                };

                resultado.Add(devolutivasEstimadas);
                resultado.Add(devolutivasRegistradas);
            }

            return resultado.OrderBy(r => r.TurmaAno);
        }

        private static string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
            => possuiFiltroUe
            ? turmaAno
            : $"{modalidade.ShortName()} - {turmaAno}";
    }
}