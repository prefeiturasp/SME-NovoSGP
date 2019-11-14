using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioParametrosSistema : RepositorioBase<ParametrosSistema>, IRepositorioParametrosSistema
    {
        public RepositorioParametrosSistema(ISgpContext database) : base(database)
        {
        }

        public string ObterValorPorNomeAno(string nome, int? ano)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select valor");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where nome = @nome");
            if (ano.HasValue)
                query.AppendLine("and ano = @ano");

            return database.Conexao.Query<string>(query.ToString(), new { nome, ano }).FirstOrDefault();
        }
    }
}