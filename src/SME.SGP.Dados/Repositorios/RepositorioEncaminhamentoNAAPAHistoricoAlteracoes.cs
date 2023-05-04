using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoNAAPAHistoricoAlteracoes : IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes
    {
        protected readonly ISgpContext database;

        protected RepositorioEncaminhamentoNAAPAHistoricoAlteracoes(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(EncaminhamentoNAAPAHistoricoAlteracoes entidade)
        {
            return (long)(await database.Conexao.InsertAsync(entidade));
        }
    }
}
