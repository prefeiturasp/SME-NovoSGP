using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoNotaFechamento : IRepositorioHistoricoNotaFechamento
    {
        private readonly ISgpContext database;
        public RepositorioHistoricoNotaFechamento(ISgpContext database)
        {
            this.database = database;
        }
    }
}
