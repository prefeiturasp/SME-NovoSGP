using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDevolutivaDiarioBordoNotificacao : IRepositorioDevolutivaDiarioBordoNotificacao
    {
        private readonly ISgpContext database;

        public RepositorioDevolutivaDiarioBordoNotificacao(ISgpContext database)
        {
            this.database = database;
        }

    }
}
