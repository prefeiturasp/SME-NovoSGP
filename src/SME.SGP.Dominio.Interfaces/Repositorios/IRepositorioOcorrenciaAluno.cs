using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaAluno
    {
        Task<long> SalvarAsync(OcorrenciaAluno entidade);
    }
}
