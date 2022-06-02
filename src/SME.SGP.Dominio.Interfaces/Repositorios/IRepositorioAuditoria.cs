using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAuditoria
    {
        Task<IEnumerable<Auditoria>> ObtenhaAuditoriaDoAdministrador(string login);
    }
}
