using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarItineranciaCommandHandler : IRequestHandler<AlterarItineranciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public AlterarItineranciaCommandHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<AuditoriaDto> Handle(AlterarItineranciaCommand request, CancellationToken cancellationToken)
        {
            await repositorioItinerancia.SalvarAsync(request.itinerancia);

            return (AuditoriaDto)request.itinerancia;
        }

        private Itinerancia MapearParaEntidade(AlterarItineranciaCommand request)
           => new Itinerancia()
           {
               Id = request.itinerancia.Id,
               
           };
    }
}
