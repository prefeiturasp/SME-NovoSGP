using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAlunoRecomendacao
    {
        Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesDoAlunoPorConselho(string alunoCodigo, int? bimestre, long fechamentoTurmaId, long[] conselhoClasseIds);
        void InserirRecomendacaoAlunoFamilia(long[] recomendacaoId, long conselhoClasseAlunoId);
        Task<IEnumerable<long>> ObterRecomendacoesDoAlunoPorConselhoAlunoId(long conselhoClasseAlunoId);
        Task ExcluirRecomendacoesPorConselhoAlunoIdRecomendacaoId(long conselhoClasseAlunoId, long[] recomendacoesId);
    }
}
