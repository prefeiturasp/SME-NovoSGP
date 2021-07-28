using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaOldCommandHandler : IRequestHandler<CalcularFrequenciaPorTurmaOldCommand, bool>
    {
        private readonly IMediator mediator;
        public readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public CalcularFrequenciaPorTurmaOldCommandHandler(
            IMediator mediator,
            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
            IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }

        public async Task<bool> Handle(CalcularFrequenciaPorTurmaOldCommand request, CancellationToken cancellationToken)
        {
            var totalAulasNaDisciplina = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, request.DisciplinaId, request.TurmaId);
            var totalAulasDaTurmaGeral = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, string.Empty, request.TurmaId);

            var ausenciasDosAlunos = await mediator.Send(new ObterAusenciasAlunosPorAlunosETurmaIdQuery(request.DataAula, request.Alunos, request.TurmaId));
            foreach (var codigoAluno in request.Alunos)
            {
                await RegistraFrequenciaPorDisciplina(request.TurmaId, request.DisciplinaId, request.DataAula, totalAulasNaDisciplina, codigoAluno, ausenciasDosAlunos);
                await RegistraFrequenciaGeral(request.TurmaId, request.DataAula, codigoAluno, totalAulasDaTurmaGeral, ausenciasDosAlunos);
            }

            return true;
        }

        private async Task RegistraFrequenciaPorDisciplina(string turmaId, string disciplinaId, DateTime dataAtual, int totalAulasNaDisciplina, string codigoAluno, System.Collections.Generic.IEnumerable<Infra.AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos)
        {
            var ausenciasAlunoPorDisciplina = ausenciasDosAlunos.FirstOrDefault(c => c.AlunoCodigo == codigoAluno && 
                                                                                     c.ComponenteCurricularId == disciplinaId);

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

                if (frequenciaAluno.TotalAusencias > frequenciaAluno.TotalAulas)
                    throw new Exception("Erro ao calcular frequência por Disciplina. Frequência Negativa");

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
                {
                    frequenciaAluno.TotalAusencias = 0;
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
                }
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

        private async Task RegistraFrequenciaGeral(string turmaId, DateTime dataAtual, string codigoAluno, int totalAulasDaTurma, System.Collections.Generic.IEnumerable<Infra.AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos)
        {
            var totalAusenciasGeralAluno = ausenciasDosAlunos.Where(c => c.AlunoCodigo == codigoAluno);
            if (totalAusenciasGeralAluno != null && totalAusenciasGeralAluno.Any())
            {
                var periodoAusencias = totalAusenciasGeralAluno.FirstOrDefault();

                var totalCompensacoesGeralAluno = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunoETurmaAsync(periodoAusencias.Bimestre, codigoAluno, string.Empty, turmaId);
                var frequenciaGeralAluno = await MapearFrequenciaAluno(codigoAluno,
                                                                    turmaId,
                                                                    string.Empty,
                                                                    periodoAusencias.PeriodoEscolarId,
                                                                    periodoAusencias.PeriodoInicio,
                                                                    periodoAusencias.PeriodoFim,
                                                                    periodoAusencias.Bimestre,
                                                                    totalAusenciasGeralAluno.Sum(a => a.TotalAusencias),
                                                                    totalAulasDaTurma,
                                                                    totalCompensacoesGeralAluno,
                                                                    TipoFrequenciaAluno.Geral);

                if (frequenciaGeralAluno.TotalAusencias > frequenciaGeralAluno.TotalAulas)
                    throw new Exception("Erro ao calcular frequência geral. Frequência Negativa");

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
