using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarEventosPorCalendarioUseCase : IListarEventosPorCalendarioUseCase
    {
        private readonly IMediator mediator;

        public ListarEventosPorCalendarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ListarEventosPorCalendarioRetornoDto>> Executar(int tipoCalendario, int anoLetivo, string codigoDre, string codigoUe, int? modalidade)
        {
            return await mediator.Send(
                new ListarEventosPorCalendarioQuery(tipoCalendario, anoLetivo, codigoDre, codigoUe, modalidade)
                );
        }
    }
}
