using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGrade : RepositorioBase<Grade>, IRepositorioGrade
    {
        public RepositorioGrade(ISgpContext conexao) : base(conexao)
        {
        }

    }
}
