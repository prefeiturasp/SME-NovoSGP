using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaCommandHandler : IRequestHandler<SalvarItineranciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public SalvarItineranciaCommandHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<AuditoriaDto> Handle(SalvarItineranciaCommand request, CancellationToken cancellationToken)
        {
            var itinerancia = MapearParaEntidade(request);

            await repositorioItinerancia.SalvarAsync(itinerancia);

            return (AuditoriaDto)itinerancia;
        }

        private Itinerancia MapearParaEntidade(SalvarItineranciaCommand request)
            => new Itinerancia()
            {
                DataVisita = request.DataVisita,
                DataRetornoVerificacao = request.DataRetornoVerificacao,
                AnoLetivo = request.AnoLetivo,
                EventoId = request.EventoId,
                DreId = request.DreId,
                UeId = request.UeId
            };
    }
}
