using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TransferirPendenciaParaNovoResponsavelUseCase : AbstractUseCase, ITransferirPendenciaParaNovoResponsavelUseCase
    {
        public TransferirPendenciaParaNovoResponsavelUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var command = mensagemRabbit.ObterObjetoMensagem<TransferirPendenciaParaNovoResponsavelCommand>();

            return await mediator.Send(command);
        }
    }
}
