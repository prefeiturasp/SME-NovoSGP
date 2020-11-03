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
    }
}
