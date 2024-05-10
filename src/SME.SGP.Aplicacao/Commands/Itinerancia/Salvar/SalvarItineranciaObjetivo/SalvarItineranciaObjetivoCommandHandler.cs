using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaObjetivoCommandHandler : IRequestHandler<SalvarItineranciaObjetivoCommand, AuditoriaDto>
    {
        private readonly IRepositorioItineranciaObjetivo repositorioItineranciaObjetivo;

        public SalvarItineranciaObjetivoCommandHandler(IRepositorioItineranciaObjetivo repositorioItineranciaObjetivo)
        {
            this.repositorioItineranciaObjetivo = repositorioItineranciaObjetivo ?? throw new ArgumentNullException(nameof(repositorioItineranciaObjetivo));
        }

        public async Task<AuditoriaDto> Handle(SalvarItineranciaObjetivoCommand request, CancellationToken cancellationToken)
        {
            var itineranciaObjetivo = MapearParaEntidade(request);

            await repositorioItineranciaObjetivo.SalvarAsync(itineranciaObjetivo);

            return (AuditoriaDto)itineranciaObjetivo;
        }
        private ItineranciaObjetivo MapearParaEntidade(SalvarItineranciaObjetivoCommand request)
            => new ItineranciaObjetivo()
            {
                ItineranciaObjetivosBaseId = request.ItineranciaObjetivoBaseId,
                ItineranciaId = request.ItineranciaId,
                Descricao = request.Descricao
            };
    }
}
