using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualPeriodoEscolar : RepositorioBase<PlanejamentoAnualPeriodoEscolar>, IRepositorioPlanejamentoAnualPeriodoEscolar
    {
        public RepositorioPlanejamentoAnualPeriodoEscolar(ISgpContext database) : base(database)
        {
        }
    }
}
