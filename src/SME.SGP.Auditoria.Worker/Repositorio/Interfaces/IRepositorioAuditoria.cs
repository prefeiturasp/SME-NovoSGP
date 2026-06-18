using SME.Pedagogico.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker.Repositorio.Interfaces
{
    public interface IRepositorioAuditoria : IRepositorioElasticBase<Entidade.Auditoria>
    {
        Task Salvar(Entidade.Auditoria auditoria);
    }
}
