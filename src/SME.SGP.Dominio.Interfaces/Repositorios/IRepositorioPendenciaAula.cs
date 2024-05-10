using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAula
    {  
        Task Salvar(long aulaId, string motivo, long pendenciaId);
        Task Excluir(long pendenciaId, long aulaId);
        void SalvarVarias(long pendenciaId, IEnumerable<long> aulas);  
    }
}