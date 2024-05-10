using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualUseCase : AbstractUseCase, IObterRegistroIndividualUseCase
    {
        public ObterRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RegistroIndividualDto> Executar(long id)
        {
            var registroIndividual = await mediator.Send(new ObterRegistroIndividualPorIdQuery(id));
          
            return registroIndividual;
        }
    }
}
