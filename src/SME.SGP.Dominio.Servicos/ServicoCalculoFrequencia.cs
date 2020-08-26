using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCalculoFrequencia : IServicoCalculoFrequencia
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IComandosProcessoExecutando comandosProcessoExecutando;

        public ServicoCalculoFrequencia(IRepositorioAula repositorioAula,
                                        IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
                                        IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                        IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
                                        IRepositorioTurma repositorioTurma,
                                        IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                        IComandosProcessoExecutando comandosProcessoExecutando)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.comandosProcessoExecutando = comandosProcessoExecutando ?? throw new ArgumentNullException(nameof(comandosProcessoExecutando));
        }

        private async Task<int> ObterBimestre(DateTime data, string turmaId)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaId);
            return await consultasPeriodoEscolar.ObterBimestre(data, turma.ModalidadeCodigo, turma.Semestre);
        }

        public async Task CalcularFrequenciaPorTurma(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId)
        {
            var bimestre = await ObterBimestre(dataAula, turmaId);

            comandosProcessoExecutando.IncluirCalculoFrequencia(turmaId, disciplinaId, bimestre).Wait();
            try
            {
                var totalAulasNaDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(dataAula, disciplinaId, turmaId);
                var totalAulasDaTurmaGeral = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(dataAula, string.Empty, turmaId);

                foreach (var codigoAluno in alunos)
                {
                    RegistraFrequenciaPorDisciplina(turmaId, disciplinaId, dataAula, totalAulasNaDisciplina, codigoAluno);
                    RegistraFrequenciaGeral(turmaId, dataAula, codigoAluno, totalAulasDaTurmaGeral);
                }
            }
            finally
            {
                comandosProcessoExecutando.ExcluirCalculoFrequencia(turmaId, disciplinaId, bimestre).Wait();
            }
        }

        private FrequenciaAluno MapearFrequenciaAluno(string codigoAluno, string turmaId, string disciplinaId, long? periodoEscolarId, DateTime periodoInicio, DateTime periodoFim, int bimestre, int totalAusencias, int totalAulas, int totalCompensacoes, TipoFrequenciaAluno tipo)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(codigoAluno, disciplinaId, periodoEscolarId.Value, tipo);
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
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(codigoAluno, dataAtual, TipoFrequenciaAluno.Geral);

                if (frequenciaAluno != null)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
        }

        private void RegistraFrequenciaPorDisciplina(string turmaId, string disciplinaId, DateTime dataAtual, int totalAulasNaDisciplina, string codigoAluno)
        {
            var ausenciasAlunoPorDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, disciplinaId, turmaId);
            if (ausenciasAlunoPorDisciplina != null)
            {
                var totalCompensacoesDisciplinaAluno = repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunoETurma(ausenciasAlunoPorDisciplina.Bimestre, codigoAluno, string.Empty, turmaId);
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
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(codigoAluno, dataAtual, TipoFrequenciaAluno.PorDisciplina);

                if (frequenciaAluno != null)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
        }
    }
}