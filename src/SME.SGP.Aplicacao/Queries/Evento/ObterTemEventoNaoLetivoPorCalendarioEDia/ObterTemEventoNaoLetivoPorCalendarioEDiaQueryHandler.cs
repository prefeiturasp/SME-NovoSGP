using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTemEventoNaoLetivoPorCalendarioEDiaQueryHandler : IRequestHandler<ObterTemEventoNaoLetivoPorCalendarioEDiaQuery, bool>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterTemEventoNaoLetivoPorCalendarioEDiaQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }
        public async Task<bool> Handle(ObterTemEventoNaoLetivoPorCalendarioEDiaQuery request, CancellationToken cancellationToken)
        {
            var taskSync = new Task<bool>(() => repositorioEvento.EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(request.TipoCalendarioId, request.DataParaVerificar, request.DreCodigo, request.UeCodigo));

            return await taskSync;
        }
    }
}
