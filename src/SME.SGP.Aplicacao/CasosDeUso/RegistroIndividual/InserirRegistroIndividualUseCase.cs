using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualUseCase : AbstractUseCase, IInserirRegistroIndividualUseCase
    {
        public InserirRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirRegistroIndividualDto param)
        {
            return await Task.FromResult(new AuditoriaDto());
        }
    }
}
