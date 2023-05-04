using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQueryHandler : IRequestHandler<ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQuery, QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO> Handle(ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterDiariosDeBordoComDevolutivasPorTurmaEAnoLetivo(request.TurmaId, request.AnoLetivo);
    }
}
