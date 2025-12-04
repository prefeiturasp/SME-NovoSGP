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
            await mediator.Send(new ExcluirSecaoAtendimentoNAAPACommand(encaminhamentoSecaoNAAPAId));

            await mediator.Send(new RegistrarHistoricoDeAlteracaoExclusaoAtendimentoNAAPACommad(encaminhamentoSecaoNAAPAId));

            await AlterarSituacaoDoAtendimento(encaminhamentoNAAPAId);

            return true;
        }

        private async Task AlterarSituacaoDoAtendimento(long encaminhamentoNAAPAId)
        {
            var existeSecao = await mediator.Send(new ExisteSecaoDeItineranciaNoAtendimentoNAAPAQuery(encaminhamentoNAAPAId));
            
            if (!existeSecao)
            {
                var encaminhamentoNAAPA = await mediator.Send(new ObterAtendimentoNAAPAPorIdQuery(encaminhamentoNAAPAId));

                await mediator.Send(new AlterarSituacaoNAAPACommand(encaminhamentoNAAPA, SituacaoNAAPA.AguardandoAtendimento));
            }
        }
    }
}
