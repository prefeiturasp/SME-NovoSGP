using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaQuestao : RepositorioBase<ItineranciaQuestao>, IRepositorioItineranciaQuestao
    {
        public RepositorioItineranciaQuestao(ISgpContext database) : base(database)
        {
        }
    }
}
