using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarEventosPorCalendarioQueryHandler : IRequestHandler<ListarEventosPorCalendarioQuery, IEnumerable<ListarEventosPorCalendarioRetornoDto>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ListarEventosPorCalendarioQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<IEnumerable<ListarEventosPorCalendarioRetornoDto>> Handle(ListarEventosPorCalendarioQuery request, CancellationToken cancellationToken)
        {
            var modalidadesCalendario = request.Modalidades.Select(c => (int)c.ObterModalidadeTipoCalendario()).Distinct().ToArray();

            return await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeEModalidades(request.TipoCalendario, request.AnoLetivo, request.CodigoDre, request.CodigoUe, modalidadesCalendario);
        }
    }
}
