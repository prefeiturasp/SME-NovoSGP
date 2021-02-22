using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaEncaminhamentoAEE : RepositorioBase<PendenciaEncaminhamentoAEE>, IRepositorioPendenciaEncaminhamentoAEE
    {
        public RepositorioPendenciaEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }
    }
}