using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase : AbstractUseCase, IExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase

    {
        public ExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var consolidado = param.ObterObjetoMensagem<ConsolidadoEncaminhamentoNAAPA>();
            await mediator.Send(new SalvarConsolidadoEncaminhamentoNAAPACommand(consolidado));
            return true;
        }
    }
}