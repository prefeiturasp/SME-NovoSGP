using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarDiariosBordoPorUeTratarUseCase : AbstractUseCase, IConsolidarDiariosBordoPorUeTratarUseCase
    {
        public ConsolidarDiariosBordoPorUeTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolidacaoDiariosBordoPorUeDto>();

            var consolidacoes = await mediator.Send(new ObterConsolidacaoDiariosBordoTurmasPorUeQuery(filtro.UeId));

            foreach (var consolidacao in consolidacoes)
                await mediator.Send(new SalvarConsolidacaoDiariosBordoCommand(consolidacao));

            return true;
        }
    }
}
