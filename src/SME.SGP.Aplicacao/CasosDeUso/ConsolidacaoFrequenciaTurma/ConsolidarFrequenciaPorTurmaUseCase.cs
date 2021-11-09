using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaPorTurmaUseCase : AbstractUseCase, IConsolidarFrequenciaPorTurmaUseCase
    {
        public ConsolidarFrequenciaPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = new FiltroConsolidacaoFrequenciaTurma();
            try
            {
                filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurma>();
                var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

                if (turma != null)
                {
                    var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(filtro.TurmaCodigo));
                    await ConsolidarFrequenciaAlunos(filtro.TurmaId, filtro.TurmaCodigo, filtro.PercentualFrequenciaMinimo, alunos);
                }

                return true;
            }
            catch (Exception ex)
            {
                string msgErro = $"ConsolidarFrequenciaPorTurma - TurmaCodigo:{filtro?.TurmaCodigo} - MensagemRabbit: {mensagem.Mensagem} - Erro:{ex.Message}";
                SentrySdk.AddBreadcrumb(msgErro);
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task ConsolidarFrequenciaAlunos(long turmaId, string turmaCodigo, double percentualFrequenciaMinimo, IEnumerable<AlunoPorTurmaResposta> alunos)
        {
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaGeralPorTurmaQuery(turmaCodigo));

            var quantidadeReprovados = frequenciaTurma?.Where(c => c.PercentualFrequencia < percentualFrequenciaMinimo).Count() ?? 0;
            var quantidadeAprovados = alunos.Count() - quantidadeReprovados;

            await RegistraConsolidacaoFrequenciaTurma(turmaId, quantidadeAprovados, quantidadeReprovados);
        }

        private async Task RegistraConsolidacaoFrequenciaTurma(long turmaId, int quantidadeAprovados, int quantidadeReprovados)
        {
            await mediator.Send(new RegistraConsolidacaoFrequenciaTurmaCommand(turmaId, quantidadeAprovados, quantidadeReprovados));
        }
    }
}
