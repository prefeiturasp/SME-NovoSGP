using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoPendenciasGeraisEventosUseCase : AbstractUseCase, IExecutaVerificacaoPendenciasGeraisEventosUseCase
    {
        public ExecutaVerificacaoPendenciasGeraisEventosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await mediator.Send(new VerificaPendenciaParametroEventoCommand());
            return true;
        }
    }
}
