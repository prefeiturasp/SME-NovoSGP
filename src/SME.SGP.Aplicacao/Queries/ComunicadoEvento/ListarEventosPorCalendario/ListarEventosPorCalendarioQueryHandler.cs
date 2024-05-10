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
    public class ListarEventosPorCalendarioQueryHandler : IRequestHandler<ListarEventosPorCalendarioQuery, IEnumerable<EventoCalendarioRetornoDto>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ListarEventosPorCalendarioQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<IEnumerable<EventoCalendarioRetornoDto>> Handle(ListarEventosPorCalendarioQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<int> modalidadesTipoCalendario;

            if (!request.Modalidades.Any(c => c == -99))
            {
                modalidadesTipoCalendario = request.Modalidades;
            }
            else
            {
                var modalidades = request.Modalidades.Select(m => (Modalidade)m);

                modalidadesTipoCalendario = modalidades.Select(m => (int)m.ObterModalidadeTipoCalendario()).Distinct().ToArray();
            }

            var eventos = await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeEModalidades(request.TipoCalendario, request.AnoLetivo, request.CodigoDre, request.CodigoUe, modalidadesTipoCalendario);
            return eventos.OrderBy(e => e.DataInicio).ToList();
        }
    }
}
