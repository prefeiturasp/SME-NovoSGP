using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresComConselhoClasseTurmaUsecase : IObterBimestresComConselhoClasseTurmaUseCase
    {
        private readonly IMediator mediator;
        public ObterBimestresComConselhoClasseTurmaUsecase(IMediator mediator) 
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> Executar(long turmaId)
        {
            return await mediator.Send(new ObterBimestresComConselhoClasseTurmaQuery(turmaId));
        }

    }
}
