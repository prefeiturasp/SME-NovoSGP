using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioHistoricoNotaConselhoClasse
    {
        Task<long> SalvarAsync(HistoricoNotaConselhoClasse historicoNotaConselhoClasse);
    }
}