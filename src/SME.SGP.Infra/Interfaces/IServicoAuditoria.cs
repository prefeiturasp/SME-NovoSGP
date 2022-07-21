using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoAuditoria
    {
        Task<bool> Auditar(Auditoria auditoria);
    }
}
