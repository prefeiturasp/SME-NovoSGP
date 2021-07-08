using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAviso : RepositorioBase<Aviso>, IRepositorioAviso
    {
        public RepositorioAviso(ISgpContext context) : base(context)
        {
        }
    }
}
