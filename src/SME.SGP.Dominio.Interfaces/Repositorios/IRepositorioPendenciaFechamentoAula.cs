using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaFechamentoAula
    {
        Task SalvarAsync(PendenciaFechamentoAula entidade);
        Task ExcluirAsync(long id);
    }
}
