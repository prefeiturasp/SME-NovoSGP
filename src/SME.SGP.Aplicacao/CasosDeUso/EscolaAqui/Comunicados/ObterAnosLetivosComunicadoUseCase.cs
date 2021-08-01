using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosComunicadoUseCase : AbstractUseCase, IObterAnosLetivosComunicadoUseCase
    {
        public ObterAnosLetivosComunicadoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<int>> Executar()
            => await mediator.Send(new ObterAnosLetivosComunicadoQuery());
    }
}
