using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaOldCommandHandler : IRequestHandler<CalcularFrequenciaPorTurmaOldCommand, bool>
    {
        private readonly IMediator mediator;
        public readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        public readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public CalcularFrequenciaPorTurmaOldCommandHandler(IMediator mediator,
                                                           IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
                                                           IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                                           IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }

        public async Task<bool> Handle(CalcularFrequenciaPorTurmaOldCommand request, CancellationToken cancellationToken)
        {
            await ValidarAlunos(request.Alunos, request.TurmaId);

            var totalAulasNaDisciplina = await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, request.DisciplinaId, request.TurmaId);
            var totalAulasDaTurmaGeral = await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, string.Empty, request.TurmaId);

            // Obtem ausencias alunos
            var ausenciasDosAlunos = await repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunosETurmaAsync(request.DataAula, request.Alunos, request.TurmaId);
            var periodosEscolaresParaFiltro = ausenciasDosAlunos.Select(a => a.PeriodoEscolarId).Distinct().ToList();
            var bimestre = ausenciasDosAlunos?.Select(a => a.Bimestre);

            // Obtem compensações alunos
            var alunosComAusencias = ausenciasDosAlunos?.Select(a => a.AlunoCodigo).Distinct();
            var compensacoesDosAlunos = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunosETurmaAsync(bimestre.ToArray(), alunosComAusencias.ToArray(), request.TurmaId);

            // Obtem calculos registrados
            var frequenciaDosAlunos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunosAsync(request.Alunos, periodosEscolaresParaFiltro, request.TurmaId);

            foreach (var codigoAluno in request.Alunos)
            {
                await RegistraFrequenciaPorDisciplina(request.TurmaId, request.DisciplinaId, totalAulasNaDisciplina, codigoAluno, ausenciasDosAlunos, compensacoesDosAlunos, frequenciaDosAlunos);
                await RegistraFrequenciaGeral(request.TurmaId, codigoAluno, totalAulasDaTurmaGeral, ausenciasDosAlunos, compensacoesDosAlunos, frequenciaDosAlunos);
            }

            // Remover duplicidade de calculos gerais que ocorrem ao rodar componentes em paralelo
            await RemoverFrequenciaGeralDulpicados(request.TurmaId, request.Alunos, frequenciaDosAlunos, ausenciasDosAlunos);

            return true;
        }

        private async Task RemoverFrequenciaGeralDulpicados(string turmaId, IEnumerable<string> alunos, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos)
        {
            var periodoEscolarId = frequenciaDosAlunos != null && frequenciaDosAlunos.Any(c => c.PeriodoEscolarId.HasValue) ?
                    frequenciaDosAlunos.FirstOrDefault(c => c.PeriodoEscolarId.HasValue).PeriodoEscolarId.Value :
                        ausenciasDosAlunos != null && ausenciasDosAlunos.Any() ?
                        ausenciasDosAlunos.First().PeriodoEscolarId.Value :
                        0;

            if (periodoEscolarId > 0)
                await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverFrequenciasDuplicadas(alunos.ToArray(), turmaId, periodoEscolarId);
        }

        private async Task ValidarAlunos(IEnumerable<string> alunos, string turmaId)
        {
            if (alunos == null || !alunos.Any())
            {
                var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(turmaId));
                if (alunosDaTurma == null || !alunosDaTurma.Any())
                    throw new NegocioException($"Não localizados alunos para turma [{turmaId}] no EOL");

                alunos = alunosDaTurma.Select(a => a.CodigoAluno).Distinct().ToList();
            }
        }

        private async Task RegistraFrequenciaPorDisciplina(string turmaId,
                                                           string disciplinaId,
                                                           int totalAulasNaDisciplina,
                                                           string codigoAluno,
                                                           IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos,
                                                           IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDosAlunos,
                                                           IEnumerable<FrequenciaAluno> frequenciaDosAlunos)
        {
            var frequenciaCalculada = ObtemFrequenciaCalculadaDoAluno(frequenciaDosAlunos, codigoAluno, disciplinaId);
            var ausenciasAluno = ObterRegistroAusenciaDoAluno(ausenciasDosAlunos, codigoAluno, disciplinaId);

            // Verifica se Aluno possui ausência na Disciplina
            if (ausenciasAluno != null)
            {
                var compensacoesAluno = ObterTotalCompensacoesDoAluno(compensacoesDosAlunos, codigoAluno, disciplinaId);

                var frequenciaAluno = frequenciaCalculada == null ?
                        new FrequenciaAluno(codigoAluno,
                                            turmaId,
                                            disciplinaId,
                                            ausenciasAluno.PeriodoEscolarId,
                                            ausenciasAluno.PeriodoInicio,
                                            ausenciasAluno.PeriodoFim,
                                            ausenciasAluno.Bimestre,
                                            ausenciasAluno.TotalAusencias,
                                            totalAulasNaDisciplina,
                                            compensacoesAluno,
                                            TipoFrequenciaAluno.PorDisciplina) :
                        frequenciaCalculada.DefinirFrequencia(
                            ausenciasAluno.TotalAusencias,
                            totalAulasNaDisciplina,
                            compensacoesAluno,
                            TipoFrequenciaAluno.PorDisciplina);

                await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
            }
            else
            {
                // Caso não tenha ausencia mas tem calculo de frequência então exclui
                if (frequenciaCalculada != null)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverAsync(frequenciaCalculada);
            }
        }

        private async Task RegistraFrequenciaGeral(string turmaId,
                                                   string codigoAluno,
                                                   int totalAulasDaTurma,
                                                   IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos,
                                                   IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDosAlunos,
                                                   IEnumerable<FrequenciaAluno> frequenciaDosAlunos)
        {
            var ausenciasDoAluno = ausenciasDosAlunos.Where(a => a.AlunoCodigo == codigoAluno);
            var frequenciaCalculada = ObtemFrequenciaCalculadaDoAluno(frequenciaDosAlunos, codigoAluno);

            // Verifica se Aluno possui ausência no Bimestre
            if (ausenciasDoAluno != null && ausenciasDoAluno.Any())
            {
                var periodoAusencia = ausenciasDoAluno.First();

                var totalAusencias = ausenciasDoAluno.Sum(a => a.TotalAusencias);
                var compensacoesAluno = ObterTotalCompensacoesDoAluno(compensacoesDosAlunos, codigoAluno);

                var frequenciaAluno = frequenciaCalculada == null ?
                        new FrequenciaAluno(codigoAluno,
                                            turmaId,
                                            "",
                                            periodoAusencia.PeriodoEscolarId,
                                            periodoAusencia.PeriodoInicio,
                                            periodoAusencia.PeriodoFim,
                                            periodoAusencia.Bimestre,
                                            totalAusencias,
                                            totalAulasDaTurma,
                                            compensacoesAluno,
                                            TipoFrequenciaAluno.Geral) :
                        frequenciaCalculada.DefinirFrequencia(
                            totalAusencias,
                            totalAulasDaTurma,
                            compensacoesAluno,
                            TipoFrequenciaAluno.Geral);

                await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
            }
            else
            {
                // Caso não tenha ausencia mas tem calculo de frequência então exclui
                if (frequenciaCalculada != null)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverAsync(frequenciaCalculada);
            }
        }

        private AusenciaPorDisciplinaAlunoDto ObterRegistroAusenciaDoAluno(IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos, string codigoAluno, string disciplinaId)
            => ausenciasDosAlunos.FirstOrDefault(c => c.AlunoCodigo == codigoAluno &&
                                                      c.ComponenteCurricularId == disciplinaId);

        private FrequenciaAluno ObtemFrequenciaCalculadaDoAluno(IEnumerable<FrequenciaAluno> frequenciaDosAlunos, string codigoAluno, string disciplinaId = "")
            => frequenciaDosAlunos.FirstOrDefault(c => c.CodigoAluno == codigoAluno &&
                                                       c.DisciplinaId == disciplinaId);

        private int ObterTotalCompensacoesDoAluno(IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDosAlunos, string codigoAluno, string disciplinaId = "")
            => (string.IsNullOrEmpty(disciplinaId) ?
            ObterCompensacoesAluno(compensacoesDosAlunos, codigoAluno)?
                .Sum(a => a.Compensacoes) :
            ObterCompensacoesAluno(compensacoesDosAlunos, codigoAluno)?
                .FirstOrDefault(c => c.ComponenteCurricularId == disciplinaId)?
                .Compensacoes) ?? 0;

        private IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> ObterCompensacoesAluno(IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDosAlunos, string codigoAluno)
            => compensacoesDosAlunos?.Where(c => c.AlunoCodigo == codigoAluno);
    }
}
