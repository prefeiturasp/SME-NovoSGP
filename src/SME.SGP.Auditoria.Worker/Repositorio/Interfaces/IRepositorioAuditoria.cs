using System.Threading.Tasks;
using SME.Pedagogico.Interface;

namespace SME.SGP.Auditoria.Worker.Repositorio.Interfaces
{
    public interface IRepositorioAuditoria : IRepositorioElasticBase<Entidade.Auditoria>
    {
        Task Salvar(Entidade.Auditoria auditoria);
    }
}
