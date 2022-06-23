using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        void SalvarVarias(IEnumerable<(Aula aula, long? planoAulaId)> aulas);
        Task ExcluirPeloSistemaAsync(long[] idsAulas);
    }
}