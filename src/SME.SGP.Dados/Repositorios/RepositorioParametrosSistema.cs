using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioParametrosSistema : RepositorioBase<ParametrosSistema>, IRepositorioParametrosSistema
    {
        public RepositorioParametrosSistema(ISgpContext database) : base(database)
        {
        }

        public string ObterValorPorTipoEAno(TipoParametroSistema tipo, int? ano = null)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select valor");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");
            if (ano.HasValue)
                query.AppendLine("and ano = @ano");

            return database.Conexao.QueryFirstOrDefault<string>(query.ToString(), new { tipo, ano });
        }
    }
}