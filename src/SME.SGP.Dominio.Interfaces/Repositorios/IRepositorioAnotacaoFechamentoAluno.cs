using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnotacaoFechamentoAluno : IRepositorioBase<AnotacaoFechamentoAluno>
    {
        Task<AnotacaoFechamentoAluno> ObterPorFechamentoEAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo);
    }
}
