using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroIndividualUseCase : AbstractUseCase, IAlterarRegistroIndividualUseCase
    {
        public AlterarRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarRegistroIndividualDto param)
        {
            return await Task.FromResult(new AuditoriaDto());
        }
    }
}
