using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAluno : IRepositorioBase<CompensacaoAusenciaAluno>
    {
        Task<bool> InserirVarios(IEnumerable<CompensacaoAusenciaAluno> registros, Usuario usuarioLogado);
        Task<bool> AlterarQuantidadeCompensacoesPorCompensacaoAlunoId(long compensacaoAusenciaAlunoId, int quantidade);
        Task<bool> ExcluirCompensacaoAusenciaAlunoPorId(long id);
        Task<bool> AlterarQuantidadeFaltasCompensadasPorId(long compensacoesAlunosAtualizar, int quantidade);
    }
}
