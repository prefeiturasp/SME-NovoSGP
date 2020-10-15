using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
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
            var modalidade = (int)((Modalidade)request.Modalidade).ObterModalidadeTipoCalendario();
            var eventos = await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeModalidadeAsync(request.TipoCalendario, request.CodigoDre, request.CodigoUe, modalidade);
            return eventos
                .Select(e => new ListarEventosPorCalendarioRetornoDto { Id = e.Id, Nome = e.Nome });
        }
    }
}
