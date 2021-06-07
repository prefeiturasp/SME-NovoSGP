using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarDevolutivasPorTurmaUseCase : AbstractUseCase, IConsolidarDevolutivasPorTurmaUseCase
    {
        public ConsolidarDevolutivasPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoDevolutivasTurma>();

                var devolutivasTurma = await mediator.Send(new ObterDevolutivaPorTurmaQuery(filtro.TurmaCodigo));

                await RegistraConsolidacaoDevolutivasTurma(filtro.TurmaId, devolutivasTurma.QuantidadeEstimadaDevolutivas, devolutivasTurma.QuantidadeRegistradaDevolutivas);

                return true;
            }
            catch (System.Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task RegistraConsolidacaoDevolutivasTurma(long turmaId, int quantidadeEstimadaDevolutivas, int quantidadeRegistradaDevolutivas)
        {
            await mediator.Send(new RegistraConsolidacaoDevolutivasTurmaCommand(turmaId, quantidadeEstimadaDevolutivas, quantidadeRegistradaDevolutivas));
        }
    }
}
