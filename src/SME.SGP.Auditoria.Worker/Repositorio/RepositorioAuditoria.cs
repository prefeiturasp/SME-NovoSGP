using Dommel;
using SME.SGP.Auditoria.Worker.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker
{
    public class RepositorioAuditoria : IRepositorioAuditoria
    {
        private readonly ISgpContext database;

        public RepositorioAuditoria(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public Task Salvar(Entidade.Auditoria auditoria)
            => database.Conexao.InsertAsync(auditoria);
    }
}
