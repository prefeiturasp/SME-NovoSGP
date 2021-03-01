using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaPlanoAEE : RepositorioBase<PendenciaPlanoAEE>, IRepositorioPendenciaPlanoAEE
    {
        public RepositorioPendenciaPlanoAEE(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
