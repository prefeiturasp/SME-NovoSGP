using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoModalidade
    {
        Task<long> SalvarAsync(ComunicadoModalidade comunicadoModalidade);
        Task<bool> ExcluirPorIdComunicado(long id);
    }
}