using SME.SGP.Metrica.Worker.Entidade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGP
    {
        // ConselhoClasse
        Task AtualizarConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId);
        Task ExcluirConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId);
        // ConselhoClasseAluno
        Task AtualizarNotasConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task AtualizarRecomendacoesConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task ExcluirTurmasComplementaresConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task AtualizarWfAprovacaoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task AtualizarParecerConclusivoConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        Task ExcluirConselhoClasseAlunoDuplicado(long conselhoClasseId, string alunoCodigo, long ultimoId);
        // ConselhoClasseNotas
        Task AtualizarWfAprovacaoConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId);
        Task AtualizarHistoricoNotaConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId);
        Task AtualizarMaiorNotaConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId);
        Task ExcluirConselhoClasseNotaDuplicado(long conselhoClasseAlunoId, long componenteCurricularId, long ultimoId);

        // FechamentoTurma
        Task AtualizarConselhoClasseFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId);
        Task AtualizarComponenteFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId);
        Task ExcluirFechamentoTurmaDuplicados(long turmaId, long periodoEscolarId, long ultimoId);
        //FechamentoTurmaDisciplina
        Task AtualizarAlunoFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId);
        Task AtualizarAnotacaoAlunoFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId);
        Task AtualizarPendenciaFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId);
        Task ExcluirFechamentoTurmaDisciplinaDuplicados(long fechamentoTurmaId, long disciplinaId, long ultimoId);
        // FechamentoAluno
        Task AtualizarNotaFechamentoAlunoDuplicados(long fechamentoDisciplinaId, string alunoCodigo, long ultimoId);
        Task AtualizarAnotacaoFechamentoAlunoDuplicados(long fechamentoDisciplinaId, string alunoCodigo, long ultimoId);
        Task ExcluirFechamentoAlunoDuplicados(long fechamentoDisciplinaId, string alunoCodigo, long ultimoId);
        // FechamentoNota
        Task AtualizarHistoricoFechamentoNotaDuplicados(long fechamentoAlunoId, long disciplinaId, long ultimoId);
        Task AtualizarWfAprovacaoFechamentoNotaDuplicados(long fechamentoAlunoId, long disciplinaId, long ultimoId);
        Task ExcluirFechamentoNotaDuplicados(long fechamentoAlunoId, long disciplinaId, long ultimoId);
        // ConsolidacaoCCAlunoTurma
        Task AtualizarNotaConsolidacaoCCAlunoTurmaDuplicado(string alunoCodigo, long turmaId, long ultimoId);
        Task ExcluirConsolidacaoCCAlunoTurmaDuplicado(string alunoCodigo, long turmaId, long ultimoId);
        // ConsolidacaoCCNota
        Task ExcluirConsolidacaoCCNotaDuplicado(long consolicacaoCCAlunoTurmaId, int bimestre, long componenteCurricularId, long ultimoId);
        // FrequenciaAluno
        Task ExcluirFrequenciaAlunoDuplicado(string turmaCodigo, string alunoCodigo, int bimestre, int tipo, string componenteCurricularId, long ultimoId);
        // RegistroFrequencia
        Task AtualizaAlunoRegistroFrequenciaDuplicado(long aulaId, long ultimoId);
        Task ExcluirRegistroFrequenciaDuplicado(long aulaId, long ultimoId);
        Task<bool> AtualizarCompensacoesRegistroFrequenciaAlunoDuplicado(long registroFrequenciaId, long aulaId, int numeroAula, string alunoCodigo, long ultimoId);
        Task AtualizarAusenciaRegistroFrequenciaAlunoDuplicado(long ultimoId);
        Task ExcluirRegistroFrequenciaAlunoDuplicado(long registroFrequenciaId, long aulaId, int numeroAula, string alunoCodigo, long ultimoId);
        Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalInconsistente>> ObterConsolidacaoFrequenciaAlunoMensalInconsistente(long turmaId);
    }
}
