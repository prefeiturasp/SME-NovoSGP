using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosItinerânciaPorTipoCalendarioUseCase : AbstractUseCase, IObterEventosItinerânciaPorTipoCalendarioUseCase
    {
        public ObterEventosItinerânciaPorTipoCalendarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<EventoNomeDto>> Executar(long tipoCalendarioId)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            return await mediator.Send(new ObterEventosItineranciaPAAIQuery(tipoCalendarioId, usuario.Login, usuario.PerfilAtual));
        }
    }
}
