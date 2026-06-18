using System.Threading.Tasks;
namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoRelatorio
    {
        Task<int> ObterTipoPorCodigo(string codigo);
    }

}
