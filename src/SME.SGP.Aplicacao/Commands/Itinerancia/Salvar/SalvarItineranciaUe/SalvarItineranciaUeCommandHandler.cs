using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaUeCommandHandler : IRequestHandler<SalvarItineranciaUeCommand, AuditoriaDto>
    {
        private readonly IRepositorioItineranciaUe repositorioItineranciaUe;

        public SalvarItineranciaUeCommandHandler(IRepositorioItineranciaUe repositorioItineranciaUe)
        {
            this.repositorioItineranciaUe = repositorioItineranciaUe ?? throw new ArgumentNullException(nameof(repositorioItineranciaUe));
        }

        public async Task<AuditoriaDto> Handle(SalvarItineranciaUeCommand request, CancellationToken cancellationToken)
        {
            var itineranciaUe = MapearParaEntidade(request);

            await repositorioItineranciaUe.SalvarAsync(itineranciaUe);

            return (AuditoriaDto)itineranciaUe;
        }
        private ItineranciaUe MapearParaEntidade(SalvarItineranciaUeCommand request)
            => new ItineranciaUe()
            {
                UeId = request.UeId,
                ItineranciaId = request.ItineranciaId
            };
    }
}
