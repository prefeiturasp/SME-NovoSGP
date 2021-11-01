using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasse : IRepositorioBase<ConselhoClasse>
    {
        Task<ConselhoClasse> ObterPorTurmaEPeriodoAsync(long turmaId, long? periodoEscolarId = null);
        Task<IEnumerable<long>> ObterConselhoClasseIdsPorTurmaEPeriodoAsync(string[] turmasCodigos, long? periodoEscolarId = null);
        Task<ConselhoClasse> ObterPorFechamentoId(long fechamentoTurmaId);
        Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> ObterimestreComConselhoClasseTurmaAsync(long turmaId);
        Task<string> ObterTurmaCodigoPorConselhoClasseId(long conselhoClasseId);
        Task<IEnumerable<string>> ObterAlunosComNotaLancadaPorConselhoClasseId(long conselhoClasseId);
        Task<IEnumerable<FechamentoConselhoClasseNotaFinalDto>> ObterNotasFechamentoOuConselhoAlunos(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<bool> AtualizarSituacao(long conselhoClasseId, SituacaoConselhoClasse situacaoConselhoClasse);
        Task<SituacaoConselhoClasse> ObterSituacaoConselhoClasse(long turmaId, long periodoEscolarId);
        Task<IEnumerable<ConselhoClasseSituacaoQuantidadeDto>> ObterConselhoClasseSituacao(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<objConsolidacaoConselhoAluno>> ObterAlunosReprocessamentoConsolidacaoConselho(int dreId);
    }

    public struct objConsolidacaoConselhoAluno
    {
        public long turmaId { get; set; }
        public int bimestre { get; set; }
        public string alunoCodigo { get; set; }
        public string erro { get; set; }

    }
}
