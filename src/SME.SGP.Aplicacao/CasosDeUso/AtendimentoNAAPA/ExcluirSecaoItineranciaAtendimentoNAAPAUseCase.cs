using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecaoItineranciaAtendimentoNAAPAUseCase : AbstractUseCase, IExcluirSecaoItineranciaAtendimentoNAAPAUseCase
    {
        public ExcluirSecaoItineranciaAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(long encaminhamentoNAAPAId, long encaminhamentoSecaoNAAPAId)
        {
            await mediator.Send(new ExcluirSecaoEncaminhamentoNAAPACommand(encaminhamentoSecaoNAAPAId));

            await mediator.Send(new RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommad(encaminhamentoSecaoNAAPAId));

            await AlterarSituacaoDoAtendimento(encaminhamentoNAAPAId);

            return true;
        }

        private async Task AlterarSituacaoDoAtendimento(long encaminhamentoNAAPAId)
        {
            var existeSecao = await mediator.Send(new ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQuery(encaminhamentoNAAPAId));
            
            if (!existeSecao)
            {
                var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAId));

                await mediator.Send(new AlterarSituacaoNAAPACommand(encaminhamentoNAAPA, SituacaoNAAPA.AguardandoAtendimento));
            }
        }
    }
}
