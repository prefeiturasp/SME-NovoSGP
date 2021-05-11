using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SME.SGP.Dominio.DateTimeExtension;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<CalcularFrequenciaPorTurmaCommand, bool>
    {
        public readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        public readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        private readonly IRepositorioProcessoExecutando repositorioProcessoExecutando;

        public CalcularFrequenciaPorTurmaCommandHandler(IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
            IRepositorioProcessoExecutando repositorioProcessoExecutando)
        {
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioProcessoExecutando = repositorioProcessoExecutando ?? throw new ArgumentNullException(nameof(repositorioProcessoExecutando));
        }
        public async Task<bool> Handle(CalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            await repositorioProcessoExecutando.SalvarAsync(new ProcessoExecutando()
            {
                Bimestre = request.Bimestre,
                DisciplinaId = request.DisciplinaId,
                TipoProcesso = TipoProcesso.CalculoFrequencia,
                TurmaId = request.TurmaId,
                CriadoEm = HorarioBrasilia()
            });

            try
            {
                var totalAulasNaDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(request.DataAula, request.DisciplinaId, request.TurmaId);
                var totalAulasDaTurmaGeral = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(request.DataAula, string.Empty, request.TurmaId);

                foreach (var codigoAluno in request.Alunos)
                {
                    RegistraFrequenciaPorDisciplina(request.TurmaId, request.DisciplinaId, request.DataAula, totalAulasNaDisciplina, codigoAluno);
                    RegistraFrequenciaGeral(request.TurmaId, request.DataAula, codigoAluno, totalAulasDaTurmaGeral);
                }
            }
            finally
            {
                var idsParaRemover = await repositorioProcessoExecutando.ObterIdsPorFiltrosAsync(request.Bimestre, request.DisciplinaId, request.TurmaId);
                await repositorioProcessoExecutando.RemoverIdsAsync(idsParaRemover.ToArray());
            }

            return true;
        }
        private void RegistraFrequenciaPorDisciplina(string turmaId, string disciplinaId, DateTime dataAtual, int totalAulasNaDisciplina, string codigoAluno)
        {
            var ausenciasAlunoPorDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, disciplinaId, turmaId);
            if (ausenciasAlunoPorDisciplina != null)
            {
                var totalCompensacoesDisciplinaAluno = repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunoETurma(ausenciasAlunoPorDisciplina.Bimestre, codigoAluno, disciplinaId, turmaId);
                var frequenciaAluno = MapearFrequenciaAluno(codigoAluno,
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
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaAluno);
                else
                if (frequenciaAluno.Id > 0)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
            else
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(codigoAluno, dataAtual, TipoFrequenciaAluno.PorDisciplina, disciplinaId);

                if (frequenciaAluno != null)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
        }
        private FrequenciaAluno MapearFrequenciaAluno(string codigoAluno, string turmaId, string disciplinaId, long? periodoEscolarId, DateTime periodoInicio, DateTime periodoFim, int bimestre, int totalAusencias, int totalAulas, int totalCompensacoes, TipoFrequenciaAluno tipo)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(codigoAluno, disciplinaId, periodoEscolarId.Value, tipo, turmaId);
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


        private void RegistraFrequenciaGeral(string turmaId, DateTime dataAtual, string codigoAluno, int totalAulasDaTurma)
        {
            var totalAusenciasGeralAluno = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, string.Empty, turmaId);
            if (totalAusenciasGeralAluno != null)
            {
                var totalCompensacoesGeralAluno = repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunoETurma(totalAusenciasGeralAluno.Bimestre, codigoAluno, string.Empty, turmaId);
                var frequenciaGeralAluno = MapearFrequenciaAluno(codigoAluno,
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
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaGeralAluno);
                else
                if (frequenciaGeralAluno.Id > 0)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaGeralAluno);
            }
            else
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(codigoAluno, dataAtual, TipoFrequenciaAluno.Geral, turmaId);

                if (frequenciaAluno != null)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
        }


    }
}