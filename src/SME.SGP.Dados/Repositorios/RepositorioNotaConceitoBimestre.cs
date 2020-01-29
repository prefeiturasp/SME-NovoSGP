using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotaConceitoBimestre : RepositorioBase<NotaConceitoBimestre>, IRepositorioNotaConceitoBimestre
    {
        public RepositorioNotaConceitoBimestre(ISgpContext database) : base(database)
        {
        }

    }
}