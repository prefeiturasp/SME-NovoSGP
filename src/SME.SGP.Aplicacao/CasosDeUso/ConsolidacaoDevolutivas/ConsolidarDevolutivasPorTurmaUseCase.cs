using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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

                var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(filtro.TurmaCodigo));

                var frequenciaTurma = await mediator.Send(new ObterFrequenciaGeralPorTurmaQuery(filtro.TurmaCodigo));

                var quantidadeEstimadaDevolutivas = 0;
                var quantidadeRegistradaDevolutivas = 0;
        
                await RegistraConsolidacaoDevolutivasTurma(filtro.TurmaId, quantidadeEstimadaDevolutivas, quantidadeRegistradaDevolutivas);


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
