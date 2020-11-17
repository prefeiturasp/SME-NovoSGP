using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioHistoricoNotaFechamento
    {
        Task<long> SalvarAsync(HistoricoNotaFechamento entidade);
    }
}