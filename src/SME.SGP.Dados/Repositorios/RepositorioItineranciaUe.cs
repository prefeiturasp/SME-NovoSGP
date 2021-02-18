using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaUe : RepositorioBase<ItineranciaUe>, IRepositorioItineranciaUe
    {
        public RepositorioItineranciaUe(ISgpContext database) : base(database)
        {
        }
    }
}
