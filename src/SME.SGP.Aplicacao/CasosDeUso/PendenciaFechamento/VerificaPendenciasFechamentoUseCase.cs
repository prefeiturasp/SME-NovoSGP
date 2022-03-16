using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPendenciasFechamentoUseCase : AbstractUseCase, IVerificaPendenciasFechamentoUseCase
    {
        public VerificaPendenciasFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<VerificaPendenciasFechamentoCommand>();

            await mediator.Send(command);

            return true;
        }
    }
}
