using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidado : IRepositorioBase<ConselhoClasseConsolidadoTurmaAluno>
    {
        Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(long turmaId, int bimestre, int situacaoConselhoClasse);
        Task<ConselhoClasseConsolidadoTurmaAluno> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(long turmaId, int bimestre, string alunoCodigo);
        Task<IEnumerable<ConsolidacaoConselhoClasseAlunoMigracaoDto>> ObterConselhoClasseConsolidadoPorTurmaAsync(string turmaId);        
    }
}
