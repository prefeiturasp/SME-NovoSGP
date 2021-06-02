using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaOldCommandHandler : IRequestHandler<CalcularFrequenciaPorTurmaOldCommand, bool>
    {
        public readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        public readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        private readonly IRepositorioProcessoExecutando repositorioProcessoExecutando;

        public CalcularFrequenciaPorTurmaOldCommandHandler(IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
            IRepositorioProcessoExecutando repositorioProcessoExecutando)
        {
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioProcessoExecutando = repositorioProcessoExecutando ?? throw new ArgumentNullException(nameof(repositorioProcessoExecutando));
        }

        public async Task<bool> Handle(CalcularFrequenciaPorTurmaOldCommand request, CancellationToken cancellationToken)
        {
            var totalAulasNaDisciplina = await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, request.DisciplinaId, request.TurmaId);
            var totalAulasDaTurmaGeral = await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, string.Empty, request.TurmaId);

            foreach (var codigoAluno in request.Alunos)
            {
                await RegistraFrequenciaPorDisciplina(request.TurmaId, request.DisciplinaId, request.DataAula, totalAulasNaDisciplina, codigoAluno);
                await RegistraFrequenciaGeral(request.TurmaId, request.DataAula, codigoAluno, totalAulasDaTurmaGeral);
            }

            return true;
        }

        private async Task RegistraFrequenciaPorDisciplina(string turmaId, string disciplinaId, DateTime dataAtual, int totalAulasNaDisciplina, string codigoAluno)
        {
            var ausenciasAlunoPorDisciplina = await repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurmaAsync(dataAtual, codigoAluno, disciplinaId, turmaId);
            if (ausenciasAlunoPorDisciplina != null)
            {
                var totalCompensacoesDisciplinaAluno = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunoETurmaAsync(ausenciasAlunoPorDisciplina.Bimestre, codigoAluno, disciplinaId, turmaId);
                var frequenciaAluno = await MapearFrequenciaAluno(codigoAluno,
                                                            turmaId,
                                                            disciplinaId,
                                                            ausenciasAlunoPorDisciplina.PeriodoEscolarId,
                                                            ausenciasAlunoPorDisciplina.PeriodoInicio,
                                                            ausenciasAlunoPorDisciplina.PeriodoFim,
                                                            ausenciasAlunoPorDisciplina.Bimestre,
                                                            ausenciasAlunoPorDisciplina.TotalAusencias,
                                                            totalAulasNaDisciplina,
                                                            totalCompensacoesDisciplinaAluno,
                                                            TipoFrequenciaAluno.PorDisciplina);

                if (frequenciaAluno.TotalAusencias > 0)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
                else
                if (frequenciaAluno.Id > 0)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverAsync(frequenciaAluno);
            }
            else
            {
                var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDataAsync(codigoAluno, dataAtual, TipoFrequenciaAluno.PorDisciplina, disciplinaId);

                if (frequenciaAluno != null)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverAsync(frequenciaAluno);
            }
        }

        private async Task<FrequenciaAluno> MapearFrequenciaAluno(string codigoAluno, string turmaId, string disciplinaId, long? periodoEscolarId, DateTime periodoInicio, DateTime periodoFim, int bimestre, int totalAusencias, int totalAulas, int totalCompensacoes, TipoFrequenciaAluno tipo)
        {
            var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterAsync(codigoAluno, disciplinaId, periodoEscolarId.Value, tipo, turmaId);
            return frequenciaAluno == null ?
            new FrequenciaAluno
                         (
                             codigoAluno,
                             turmaId,
                             disciplinaId,
                             periodoEscolarId,
                             periodoInicio,
                             periodoFim,
                             bimestre,
                             totalAusencias,
                             totalAulas,
                             totalCompensacoes,
                             tipo
                         ) : frequenciaAluno.DefinirFrequencia(totalAusencias, totalAulas, totalCompensacoes, tipo);
        }

        private async Task RegistraFrequenciaGeral(string turmaId, DateTime dataAtual, string codigoAluno, int totalAulasDaTurma)
        {
            var totalAusenciasGeralAluno = await repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurmaAsync(dataAtual, codigoAluno, string.Empty, turmaId);
            if (totalAusenciasGeralAluno != null)
            {
                var totalCompensacoesGeralAluno = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunoETurmaAsync(totalAusenciasGeralAluno.Bimestre, codigoAluno, string.Empty, turmaId);
                var frequenciaGeralAluno = await MapearFrequenciaAluno(codigoAluno,
                                                                    turmaId,
                                                                    string.Empty,
                                                                    totalAusenciasGeralAluno.PeriodoEscolarId,
                                                                    totalAusenciasGeralAluno.PeriodoInicio,
                                                                    totalAusenciasGeralAluno.PeriodoFim,
                                                                    totalAusenciasGeralAluno.Bimestre,
                                                                    totalAusenciasGeralAluno.TotalAusencias,
                                                                    totalAulasDaTurma,
                                                                    totalCompensacoesGeralAluno,
                                                                    TipoFrequenciaAluno.Geral);

                if (frequenciaGeralAluno.PercentualFrequencia < 100)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaGeralAluno);
                else
                if (frequenciaGeralAluno.Id > 0)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverAsync(frequenciaGeralAluno);
            }
            else
            {
                var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDataAsync(codigoAluno, dataAtual, TipoFrequenciaAluno.Geral);

                if (frequenciaAluno != null)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverAsync(frequenciaAluno);
            }
        }
    }
}
