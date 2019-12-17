using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativaDisciplina : IRepositorioBase<AtividadeAvaliativaDisciplina>
    {
        Task<IEnumerable<AtividadeAvaliativaDisciplina>> ListarPorIdAtividade(long idAtividadeAvaliativa);
    }
}
