using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoNotaFechamento : IRepositorioHistoricoNotaFechamento
    {
        private readonly ISgpContext database;
        public RepositorioHistoricoNotaFechamento(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(HistoricoNotaFechamento entidade)
        {
            entidade.Id = (await database.Conexao.InsertMappedAsync(entidade));

            return entidade.Id;
        }
    }
}
