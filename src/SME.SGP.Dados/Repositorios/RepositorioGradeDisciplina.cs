using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGradeDisciplina : RepositorioBase<GradeDisciplina>, IRepositorioGradeDisciplina
    {
        public RepositorioGradeDisciplina(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
