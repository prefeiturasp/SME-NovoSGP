using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresComConselhoClasseTurmaUsecase : AbstractUseCase, IObterBimestresComConselhoClasseTurmaUseCase
    {
        public ObterBimestresComConselhoClasseTurmaUsecase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> Executar(long turmaId)
        {
            return null;
        }

    }
}
