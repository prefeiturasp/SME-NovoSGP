using MediatR;
using Sentry;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase : AbstractUseCase, IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroAcompanhamentoAprendizagemAlunoTurmaDTO>();

                var totalAlunosComAcompanhamento = await mediator.Send(new ObterTotalAlunosComAcompanhamentoQuery(filtro.TurmaId, filtro.AnoLetivo, filtro.Semestre));

                // TODO: Implementar
                var totalAlunosSemAcompanhamento = 0;

                await mediator.Send(new RegistraConsolidacaoAcompanhamentoAprendizagemCommand(filtro.TurmaId, totalAlunosComAcompanhamento, totalAlunosSemAcompanhamento, filtro.Semestre));

                return true;
            }
            catch (System.Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }
    }
}
