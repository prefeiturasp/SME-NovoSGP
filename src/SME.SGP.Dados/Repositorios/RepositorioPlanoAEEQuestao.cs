using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEQuestao : RepositorioBase<PlanoAEEQuestao>, IRepositorioPlanoAEEQuestao
    {
        public RepositorioPlanoAEEQuestao(ISgpContext database) : base(database)
        {
        }
    }
}
