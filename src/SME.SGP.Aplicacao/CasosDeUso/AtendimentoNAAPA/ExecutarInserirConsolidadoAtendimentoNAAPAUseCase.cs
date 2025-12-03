using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutarInserirConsolidadoAtendimentoNAAPAUseCase : AbstractUseCase,IExecutarInserirConsolidadoAtendimentoNAAPAUseCase

    {
        public ExecutarInserirConsolidadoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var consolidado = param.ObterObjetoMensagem<ConsolidadoEncaminhamentoNAAPA>();
            await mediator.Send(new SalvarConsolidadoAtendimentoNAAPACommand(consolidado));
            return true;
        }
    }
}