using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPendenciaRegistroIndividualUseCase : AbstractUseCase, IAtualizarPendenciaRegistroIndividualUseCase
    {
        public AtualizarPendenciaRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<AtualizarPendenciaRegistroIndividualCommand>();
            await mediator.Send(command);
            return true;
        }
    }
}