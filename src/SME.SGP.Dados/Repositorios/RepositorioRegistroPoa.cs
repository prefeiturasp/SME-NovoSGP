using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroPoa : RepositorioBase<RegistroPoa>, IRepositorioRegistroPoa
    {
        public RepositorioRegistroPoa(ISgpContext sgpContext) : base(sgpContext)
        {
        }
    }
}