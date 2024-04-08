using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroColetivoUseCase : AbstractUseCase, IExcluirRegistroColetivoUseCase
    {
        public ExcluirRegistroColetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<bool> Executar(long param)
        {
            return mediator.Send(new RemoverRegistroColetivoCommand(param));
        }
    }
}
