using MediatR;
using Sentry;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaRecorrenteUseCase : AbstractUseCase, IInserirAulaRecorrenteUseCase
    {
        public InserirAulaRecorrenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem InserirAulaRecorrenteUseCase", "Rabbit - InserirAulaRecorrenteUseCase");
            InserirAulaRecorrenteCommand command = mensagemRabbit.ObterObjetoMensagem<InserirAulaRecorrenteCommand>();

            return await mediator.Send(command);
        }
    }
}
