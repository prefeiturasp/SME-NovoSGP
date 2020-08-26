using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTemEventoLetivoPorCalendarioEDiaQueryHandler : IRequestHandler<ObterTemEventoLetivoPorCalendarioEDiaQuery, bool>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterTemEventoLetivoPorCalendarioEDiaQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }
        public Task<bool> Handle(ObterTemEventoLetivoPorCalendarioEDiaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(repositorioEvento.EhEventoLetivoPorTipoDeCalendarioDataDreUe(request.TipoCalendarioId, request.DataParaVerificar, request.DreCodigo, request.UeCodigo));
        }
    }
}
