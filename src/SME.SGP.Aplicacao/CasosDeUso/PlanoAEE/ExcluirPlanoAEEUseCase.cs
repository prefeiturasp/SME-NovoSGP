using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAEEUseCase : AbstractUseCase, IExcluirPlanoAEEUseCase
    {
        public ExcluirPlanoAEEUseCase(IMediator mediator) : base(mediator) { }
        public async Task<bool> Executar(long planoAEEId)
        {
            return (await mediator.Send(new ExcluirPlanoAEECommand(planoAEEId)));
        }
    }
}
