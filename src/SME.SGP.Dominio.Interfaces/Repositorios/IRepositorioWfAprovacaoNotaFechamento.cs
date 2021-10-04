using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWfAprovacaoNotaFechamento
    {
        Task SalvarAsync(WfAprovacaoNotaFechamento entidade);
    }
}
