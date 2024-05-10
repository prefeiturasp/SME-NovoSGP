using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseConsulta : IRepositorioBase<ConselhoClasse>
    {
        Task<ConselhoClasse> ObterPorTurmaEPeriodoAsync(long turmaId, long? periodoEscolarId = null);
        Task<IEnumerable<long>> ObterConselhoClasseIdsPorTurmaEPeriodoAsync(string[] turmasCodigos, long? periodoEscolarId = null);
        Task<IEnumerable<long>> ObterConselhoClasseIdsPorTurmaEBimestreAsync(string[] turmasCodigos, long? bimestre = null);
        Task<ConselhoClasse> ObterPorFechamentoId(long fechamentoTurmaId);
        Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> ObterBimestreComConselhoClasseTurmaAsync(long turmaId);
        Task<IEnumerable<string>> ObterAlunosComNotaLancadaPorConselhoClasseId(long conselhoClasseId);
        Task<IEnumerable<FechamentoConselhoClasseNotaFinalDto>> ObterNotasFechamentoOuConselhoAlunos(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<SituacaoConselhoClasse> ObterSituacaoConselhoClasse(long turmaId, long periodoEscolarId);
        Task<IEnumerable<ConselhoClasseSituacaoQuantidadeDto>> ObterConselhoClasseSituacao(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<objConsolidacaoConselhoAluno>> ObterAlunosReprocessamentoConsolidacaoConselho(int dreId);
        Task<ConselhoClasse> ObterConselhoClassePorId(long conselhoClasseId);
        Task<ConselhoClasse> ObterPorAlunoEPeriodoAsync(string alunoCodigo, long? periodoEscolarId = null);
        Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> ObterTotalAulasPorAlunoTurma(string codigoAluno, string codigoTurma);
        Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> ObterTotalAulasSemFrequenciaPorTurma(string discplinaId, string codigoTurma);
        Task<IEnumerable<TotalAulasNaoLancamNotaDto>> ObterTotalAulasNaoLancamNotaPorBimestreTurma(string codigoTurma, int bimestre, string codigoAluno);
        Task<IEnumerable<int>> ObterTotalAulasSemFrequenciaPorTurmaBimestre(string disciplinaId, string codigoTurma, int bimestre, DateTime dataMatricula);
        Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> ObterTotalCompensacoesComponenteNaoLancaNotaPorBimestre(string codigoTurma, int bimestre);
        Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> ObterTotalCompensacoesComponenteNaoLancaNota(string codigoTurma);
        Task<bool> ExisteConselhoDeClasseParaTurma(string[] codigosTurmas, int bimestre);
    }

    public struct objConsolidacaoConselhoAluno
    {
        public long turmaId { get; set; }
        public int bimestre { get; set; }
        public string alunoCodigo { get; set; }
        public string erro { get; set; }

    }
}
