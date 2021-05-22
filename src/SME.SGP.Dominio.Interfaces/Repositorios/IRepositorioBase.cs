using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        IEnumerable<T> Listar();

        T ObterPorId(long id);

        Task<T> ObterPorIdAsync(long id);

        void Remover(long id);

        void Remover(T entidade);
        
        Task RemoverAsync(T entidade);

        long Salvar(T entidade);

        Task<long> SalvarAsync(T entidade);

        Task<bool> Exists(long id, string coluna = null);

        Task<long> RemoverLogico(long id, string coluna = null);      
    }
}