using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaFechamentoAtividadeAvaliativa
    {
        Task SalvarAsync(PendenciaFechamentoAtividadeAvaliativa entidade);
        Task ExcluirAsync(long id);
    }
}
