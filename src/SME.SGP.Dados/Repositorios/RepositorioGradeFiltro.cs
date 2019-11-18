using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGradeFiltro : RepositorioBase<GradeFiltro>, IRepositorioGradeFiltro
    {
        public RepositorioGradeFiltro(ISgpContext conexao) : base(conexao)
        {
        }
    }
}