using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaObjetivoCommandHandler : IRequestHandler<ExcluirItineranciaObjetivoCommand, bool>
    {
        private readonly IRepositorioItineranciaObjetivo repositorioItineranciaObjetivo;

        public ExcluirItineranciaObjetivoCommandHandler(IRepositorioItineranciaObjetivo repositorioItineranciaObjetivo)
        {
            this.repositorioItineranciaObjetivo = repositorioItineranciaObjetivo ?? throw new ArgumentNullException(nameof(repositorioItineranciaObjetivo));
        }

        public async Task<bool> Handle(ExcluirItineranciaObjetivoCommand request, CancellationToken cancellationToken)
        {
            var id = await repositorioItineranciaObjetivo.RemoverLogico(request.Id);

            return id != 0;
        }
    }
}
