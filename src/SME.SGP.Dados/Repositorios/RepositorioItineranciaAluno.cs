using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaAluno : RepositorioBase<ItineranciaAluno>, IRepositorioItineranciaAluno
    {
        public RepositorioItineranciaAluno(ISgpContext database) : base(database)
        {
        }
    }
}
