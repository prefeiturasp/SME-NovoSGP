using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoNotaConselhoClasse : IRepositorioHistoricoNotaConselhoClasse
    {
        private readonly ISgpContext database;

        public RepositorioHistoricoNotaConselhoClasse(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(HistoricoNotaConselhoClasse historicoNotaConselhoClasse)
        {
            historicoNotaConselhoClasse.Id = (long)(await database.Conexao.InsertAsync(historicoNotaConselhoClasse));

            return historicoNotaConselhoClasse.Id;
        }
    }
}
