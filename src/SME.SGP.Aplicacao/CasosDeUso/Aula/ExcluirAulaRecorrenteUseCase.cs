using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaRecorrenteUseCase : AbstractUseCase, IExcluirAulaRecorrenteUseCase
    {
        public ExcluirAulaRecorrenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<ExcluirAulaRecorrenteCommand>();

            return await mediator.Send(command);
        }
    }
}
