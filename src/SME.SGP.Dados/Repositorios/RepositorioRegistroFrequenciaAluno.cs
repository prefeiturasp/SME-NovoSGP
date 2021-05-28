using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioRegistroFrequenciaAluno : IRepositorioRegistroFrequenciaAluno
    {
        private readonly ISgpContext database;

        public RepositorioRegistroFrequenciaAluno(ISgpContext database)
        {
            this.database = database;
        }
    }
}
