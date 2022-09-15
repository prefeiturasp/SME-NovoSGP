using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarConselhoClasseUseCase: AbstractUseCase, IConsolidarConselhoClasseUseCase
    {
        public ConsolidarConselhoClasseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(int dreId)
        {
            await mediator.Send(new ConsolidarConselhoClasseCommand(dreId));

            return true;
        }
    }
}