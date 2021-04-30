using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaTurma : RepositorioBase<ConsolidacaoFrequenciaTurma>, IRepositorioConsolidacaoFrequenciaTurma
    {
        public RepositorioConsolidacaoFrequenciaTurma(ISgpContext database) : base(database)
        {
        }
    }
}