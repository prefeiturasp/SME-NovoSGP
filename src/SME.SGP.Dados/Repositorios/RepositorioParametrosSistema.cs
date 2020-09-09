using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioParametrosSistema : RepositorioBase<ParametrosSistema>, IRepositorioParametrosSistema
    {
        public RepositorioParametrosSistema(ISgpContext database) : base(database)
        {
        }

        public async Task AtualizarValorPorTipoAsync(TipoParametroSistema tipo, string valor, int? ano = null)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("update parametros_sistema set valor = @valor");
            query.AppendLine("where ativo and tipo = @tipo");
            if (ano.HasValue)
                query.AppendLine("and ano = @ano");

            await database.Conexao.ExecuteAsync(query.ToString(), new { tipo, valor, ano });
        }

        public IEnumerable<KeyValuePair<string, string>> ObterChaveEValorPorTipo(TipoParametroSistema tipo)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select nome as Key, valor as Value");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");

            return database.Conexao.Query<KeyValuePair<string, string>>(query.ToString(), new { tipo })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public KeyValuePair<string, string>? ObterUnicoChaveEValorPorTipo(TipoParametroSistema tipo)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select nome as Key, valor as Value");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");

            return database.Conexao.QueryFirstOrDefault<KeyValuePair<string, string>>(query.ToString(), new { tipo });
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