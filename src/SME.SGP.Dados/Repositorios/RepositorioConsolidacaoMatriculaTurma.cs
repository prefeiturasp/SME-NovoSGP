using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidacaoMatriculaTurma : IRepositorioConsolidacaoMatriculaTurma
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoMatriculaTurma(ISgpContext database)
        {
            this.database = database;
        }
    }
}
