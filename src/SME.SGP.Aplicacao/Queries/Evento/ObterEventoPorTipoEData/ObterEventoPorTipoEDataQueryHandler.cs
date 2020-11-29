using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventoPorTipoEDataQueryHandler : IRequestHandler<ObterEventoPorTipoEDataQuery, IEnumerable<Evento>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterEventoPorTipoEDataQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<IEnumerable<Evento>> Handle(ObterEventoPorTipoEDataQuery request, CancellationToken cancellationToken)
            => await repositorioEvento.ObterEventosPorTipoEData(request.TipoEvento, request.Data);
    }
}
