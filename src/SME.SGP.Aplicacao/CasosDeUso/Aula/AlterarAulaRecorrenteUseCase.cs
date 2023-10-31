using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaRecorrenteUseCase : AbstractUseCase, IAlterarAulaRecorrenteUseCase
    {
        public AlterarAulaRecorrenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            AlterarAulaRecorrenteCommand command = param.ObterObjetoMensagem<AlterarAulaRecorrenteCommand>();

            return await mediator.Send(command);
        }
    }
}
