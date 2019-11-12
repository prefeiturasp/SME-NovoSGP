using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public bool UsuarioPodeCriarAula(Aula aula, Usuario usuario)
        {
            var query = "";
            return database.Conexao.QueryFirst<bool>(query, new
            {
                aula.TurmaId,
                aula.UeId
            });
        }
    }
}