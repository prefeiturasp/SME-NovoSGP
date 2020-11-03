using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
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
            int? modalidade = null;
            if (request.Modalidade.HasValue)
            {
                modalidade = (int)((Modalidade)request.Modalidade).ObterModalidadeTipoCalendario();
            }
            return await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeModalidadeAsync(request.TipoCalendario, request.AnoLetivo, request.CodigoDre, request.CodigoUe, modalidade);
        }
    }
}
