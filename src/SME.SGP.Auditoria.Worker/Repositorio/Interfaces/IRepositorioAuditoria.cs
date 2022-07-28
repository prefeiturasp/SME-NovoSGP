using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker.Interfaces
{
    public interface IRepositorioAuditoria
    {
        Task Salvar(Entidade.Auditoria auditoria);
    }
}
