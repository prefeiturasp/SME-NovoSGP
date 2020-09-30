using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresComConselhoClasseTurmaUseCase : IObterBimestresComConselhoClasseTurmaUseCase
    {
        private readonly IMediator mediator;
        public ObterBimestresComConselhoClasseTurmaUseCase(IMediator mediator) 

        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> Executar(long turmaId)
        {
            return await mediator.Send(new ObterBimestresComConselhoClasseTurmaQuery(turmaId));
        }

    }
}
