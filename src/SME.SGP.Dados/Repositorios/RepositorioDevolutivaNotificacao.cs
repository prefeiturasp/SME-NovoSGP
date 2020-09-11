using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDevolutivaNotificacao : IRepositorioDevolutivaNotificacao
    {
        private readonly ISgpContext database;

        public RepositorioDevolutivaNotificacao(ISgpContext database)
        {
            this.database = database;
        }

    }
}
