using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoNAAPAAuditoria : IRepositorioEncaminhamentoNAAPAAuditoria
    {
        protected readonly ISgpContext database;

        protected RepositorioEncaminhamentoNAAPAAuditoria(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(EncaminhamentoNAAPAAuditoria entidade)
        {
            return (long)(await database.Conexao.InsertAsync(entidade));
        }
    }
}
