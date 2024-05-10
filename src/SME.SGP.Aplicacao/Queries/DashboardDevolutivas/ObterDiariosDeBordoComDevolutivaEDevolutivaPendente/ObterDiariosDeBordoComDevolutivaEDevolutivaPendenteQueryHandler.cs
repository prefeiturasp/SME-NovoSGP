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
    public class ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQueryHandler : IRequestHandler<ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery, IEnumerable<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo;
        }

        public async Task<IEnumerable<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>> Handle(ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery request, CancellationToken cancellationToken)
        {
            var resultado = new List<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>();

            var diariosDeBordoPorTurmaAno = await repositorioDiarioBordo.ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteAsync(request.AnoLetivo, request.Modalidade, request.DataAula, request.DreId, request.UeId);
            if (!diariosDeBordoPorTurmaAno?.Any() ?? true) return resultado;

            foreach (var diarioDeBordoPorTurmaAno in diariosDeBordoPorTurmaAno)
            {
                var diariosComDevolutiva = new GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto
                {
                    Descricao = DashboardDevolutivasConstants.QuantidadeComDevolutiva,
                    Quantidade = diarioDeBordoPorTurmaAno.DiariosComDevolutivas,
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId.HasValue, diarioDeBordoPorTurmaAno.TurmaAno, request.Modalidade)
                };

                var diariosComDevolutivaPendente = new GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto
                {
                    Descricao = DashboardDevolutivasConstants.QuantidadeComDevolutivaPendente,
                    Quantidade = diarioDeBordoPorTurmaAno.DiariosComDevolutivasPendentes,
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId.HasValue, diarioDeBordoPorTurmaAno.TurmaAno, request.Modalidade)
                };

                resultado.Add(diariosComDevolutiva);
                resultado.Add(diariosComDevolutivaPendente);
            }

            return resultado;
        }

        private static string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
            => possuiFiltroUe
            ? turmaAno
            : $"{modalidade.ShortName()} - {turmaAno}";
    }
}