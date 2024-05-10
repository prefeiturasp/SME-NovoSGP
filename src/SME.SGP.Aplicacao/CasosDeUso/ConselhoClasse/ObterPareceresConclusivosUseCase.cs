using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosUseCase : IObterPareceresConclusivosUseCase
    {
        private readonly IMediator mediator;

        public ObterPareceresConclusivosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivoDto>> Executar()
        {
            return await mediator.Send(new ObterPareceresConclusivosQuery());
        }
    }
}
