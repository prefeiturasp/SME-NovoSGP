using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoNAAPAAuditoria
    {
        Task<long> SalvarAsync(EncaminhamentoNAAPAAuditoria entidade);
    }
}
