using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaObjetivo : RepositorioBase<ItineranciaObjetivo>, IRepositorioItineranciaObjetivo
    {
        public RepositorioItineranciaObjetivo(ISgpContext database) : base(database)
        {
        }
    }
}
