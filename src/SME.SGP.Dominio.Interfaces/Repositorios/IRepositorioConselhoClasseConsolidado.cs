using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidado : IRepositorioBase<ConselhoClasseConsolidadoTurmaAluno>
    {
        Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(long turmaId, int situacaoConselhoClasse);
        Task<ConselhoClasseConsolidadoTurmaAluno> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(long turmaId, string alunoCodigo);
        Task<IEnumerable<long>> ObterConsolidacoesAtivasIdPorAlunoETurmaAsync(string alunoCodigo, long turmaId);

        Task<IEnumerable<ConsolidacaoConselhoClasseAlunoMigracaoDto>> ObterFechamentoNotaAlunoOuConselhoClasseAsync(long turmaId);
        Task<long> ObterConselhoClasseConsolidadoPorTurmaAlunoAsync(long turmaId, string alunoCodigo);
        Task<bool> ExcluirLogicamenteConsolidacaoConselhoClasseAlunoTurmaPorIdConsolidacao(long[] idsConsolidacoes);
    }
}
