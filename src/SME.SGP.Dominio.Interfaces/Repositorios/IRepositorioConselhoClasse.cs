using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasse : IRepositorioBase<ConselhoClasse>
    {
        Task<bool> AtualizarSituacao(long conselhoClasseId, SituacaoConselhoClasse situacaoConselhoClasse);

        Task<IEnumerable<ConselhoClasseAlunosNotaPorFechamentoIdDto>> ObterConselhoClasseAlunosNotaPorFechamentoId(long fechamentoTurmaId);
        Task<IEnumerable<AlunoTemRecomandacaoDto>> VerificarSeExisteRecomendacaoPorTurma(string[] turmasId, int bimestre);
        Task<IEnumerable<ConselhoClasseAlunoNotaDto>> ObterConselhoClasseAlunoNota(string[] turmasCodigos, int bimestre);
    }
}
