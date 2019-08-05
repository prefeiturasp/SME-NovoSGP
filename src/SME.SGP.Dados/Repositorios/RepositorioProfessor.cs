using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProfessor : RepositorioBase<Professor>, IRepositorioProfessor
    {
        public RepositorioProfessor(SgpContext conexao) : base(conexao)
        {
        }
    }
}