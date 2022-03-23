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

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            AlterarAulaRecorrenteCommand command = mensagemRabbit.ObterObjetoMensagem<AlterarAulaRecorrenteCommand>();

            return await mediator.Send(command);
        }
    }
}
