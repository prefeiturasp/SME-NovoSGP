using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAvisoMuralUseCase : AbstractUseCase, IAlterarAvisoMuralUseCase
    {
        public AlterarAvisoMuralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(long avisoId, string mensagem)
        {
            await mediator.Send(new AlterarAvisoDoMuralCommand(avisoId, mensagem));
        }
    }
}
