using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEE : IRepositorioBase<PlanoAEE>
    {
        Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao);
    }
}
