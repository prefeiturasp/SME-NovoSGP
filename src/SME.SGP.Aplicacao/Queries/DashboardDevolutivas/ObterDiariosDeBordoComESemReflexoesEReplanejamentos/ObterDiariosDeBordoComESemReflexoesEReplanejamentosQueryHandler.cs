using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoComESemReflexoesEReplanejamentosQueryHandler : IRequestHandler<ObterDiariosDeBordoComESemReflexoesEReplanejamentosQuery, IEnumerable<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiariosDeBordoComESemReflexoesEReplanejamentosQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo;
        }

        public async Task<IEnumerable<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>> Handle(ObterDiariosDeBordoComESemReflexoesEReplanejamentosQuery request, CancellationToken cancellationToken)
        {
            var resultado = new List<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>();

            var diariosDeBordoPorTurmaAno = await repositorioDiarioBordo.ObterDiariosDeBordoComESemReflexoesEReplanejamentosAsync(request.AnoLetivo, request.Modalidade, request.DataAula, request.DreId, request.UeId);
            if (!diariosDeBordoPorTurmaAno?.Any() ?? true) return resultado;

            foreach (var diarioDeBordoPorTurmaAno in diariosDeBordoPorTurmaAno)
            {
                var diariosComReflexoesEReplanejamento = new GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto
                {
                    Descricao = DashboardDevolutivasConstants.QuantidadeComReflexoesEReplanejamento,
                    Quantidade = diarioDeBordoPorTurmaAno.DiariosComReflexoesEReplanejamento,
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId.HasValue, diarioDeBordoPorTurmaAno.TurmaAno, request.Modalidade)
                };

                var diariosSemReflexoesEReplanejamento = new GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto
                {
                    Descricao = DashboardDevolutivasConstants.QuantidadeSemReflexoesEReplanejamento,
                    Quantidade = diarioDeBordoPorTurmaAno.DiariosSemReflexoesEReplanejamento,
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId.HasValue, diarioDeBordoPorTurmaAno.TurmaAno, request.Modalidade)
                };

                resultado.Add(diariosComReflexoesEReplanejamento);
                resultado.Add(diariosSemReflexoesEReplanejamento);
            }

            return resultado;
        }

        private static string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
            => possuiFiltroUe
            ? turmaAno
            : $"{modalidade.ShortName()} - {turmaAno}";
    }
}