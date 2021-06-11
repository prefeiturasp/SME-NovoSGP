using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaCommandHandler : IRequestHandler<ExcluirItineranciaCommand, bool>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ExcluirItineranciaCommandHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new System.ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<bool> Handle(ExcluirItineranciaCommand request, CancellationToken cancellationToken)
        {
            var id = await repositorioItinerancia.RemoverLogico(request.Id);

            return id != 0;
        }
    }
}
