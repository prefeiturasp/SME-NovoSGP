using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterDataPossuiEventoLiberacaoExcepcionalQueryHandler : IRequestHandler<ObterDataPossuiEventoLiberacaoExcepcionalQuery, bool>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterDataPossuiEventoLiberacaoExcepcionalQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<bool> Handle(ObterDataPossuiEventoLiberacaoExcepcionalQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEvento.DataPossuiEventoLiberacaoExcepcionalAsync(request.TipoCalendarioId, request.Data, request.CodigoUe);
        }
    }
}
