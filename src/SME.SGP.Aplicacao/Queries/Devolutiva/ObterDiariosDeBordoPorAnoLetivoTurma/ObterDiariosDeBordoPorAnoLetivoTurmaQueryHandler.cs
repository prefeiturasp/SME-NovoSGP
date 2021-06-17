using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoPorAnoLetivoTurmaQueryHandler : IRequestHandler<ObterDiariosDeBordoPorAnoLetivoTurmaQuery, QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterDiariosDeBordoPorAnoLetivoTurmaQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO> Handle(ObterDiariosDeBordoPorAnoLetivoTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterDiariosDeBordoPorTurmaEAnoLetivo(request.TurmaCodigo, request.AnoLetivo);
    }
}
