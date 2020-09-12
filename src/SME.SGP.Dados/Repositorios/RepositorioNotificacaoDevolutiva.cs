using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoDevolutiva : IRepositorioNotificacaoDevolutiva
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoDevolutiva(ISgpContext database)
        {
            this.database = database;
        }

    }
}
