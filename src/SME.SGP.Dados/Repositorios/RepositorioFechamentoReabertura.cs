using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoReabertura : RepositorioBase<FechamentoReabertura>, IRepositorioFechamentoReabertura
    {
        protected readonly ISgpContext database;

        public RepositorioFechamentoReabertura(ISgpContext database) : base(database)
        {
            this.database = database;
        }
    }
}