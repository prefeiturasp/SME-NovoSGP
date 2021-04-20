using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioPerfilEventoTipo : RepositorioBase<PerfilEventoTipo>, IRepositorioPerfilEventoTipo
    {
        public RepositorioPerfilEventoTipo(ISgpContext context) : base(context)
        {
        }
    }
}
