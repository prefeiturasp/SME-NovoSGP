using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioParametrosSistemaConsulta : RepositorioBase<ParametrosSistema>, IRepositorioParametrosSistemaConsulta
    {
        public RepositorioParametrosSistemaConsulta(ISgpContextConsultas database) : base(database)
        {
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> ObterChaveEValorPorTipoEAno(TipoParametroSistema tipo, int ano)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select nome as Key, valor as Value");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");
            query.AppendLine("and ano = @ano");

            var resultado = await database.Conexao.QueryAsync<KeyValuePair<string, string>>(query.ToString(), new { tipo, ano });

            return resultado
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public async Task<KeyValuePair<string, string>?> ObterUnicoChaveEValorPorTipo(TipoParametroSistema tipo)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select nome as Key, valor as Value");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");

            return await database.Conexao
                .QueryFirstAsync<KeyValuePair<string, string>>(query.ToString(), new { tipo });
        }

        public async Task<IEnumerable<ParametrosSistema>> ObterPorTiposAsync(long[] tipos)
        {
            var query = @"SELECT * FROM PARAMETROS_SISTEMA WHERE TIPO = ANY(@tipos) AND ATIVO = TRUE";


            return await database.Conexao.QueryAsync<ParametrosSistema>(query.ToString(), new { tipos });
        }

        public async Task<string> ObterValorPorTipoEAno(TipoParametroSistema tipo, int? ano = null)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select valor");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");
            if (ano.HasValue)
                query.AppendLine("and ano = @ano");

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query.ToString(), new { tipo, ano });
        }

        public async Task<string> ObterValorUnicoPorTipo(TipoParametroSistema tipoParametroSistema)
        {

            var query = @"select valor
                          from parametros_sistema
                         where tipo = @tipoParametroSistema and ativo";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { tipoParametroSistema });            
        }

        public async Task<T> ObterValorUnicoPorTipo<T>(TipoParametroSistema tipoParametroSistema)
        {

            var query = @"select valor
                          from parametros_sistema
                         where tipo = @tipoParametroSistema and ativo";

            return await database.Conexao.QueryFirstAsync<T>(query, new { tipoParametroSistema });
        }
        public async Task<bool> VerificaSeExisteParametroSistemaPorAno(int ano)
        {
            var query = @"select 1
                            from parametros_sistema ps
                           where ano = @ano
                             and ativo ";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ano });
        }

        public async Task<ParametrosSistema> ObterParametroPorTipoEAno(TipoParametroSistema tipo, int ano = 0)
        {
            var query = @"select *
                            from parametros_sistema ps
                           where ano = @ano
                             and tipo = @tipo";

            return await database.Conexao.QueryFirstOrDefaultAsync<ParametrosSistema>(query, new { tipo, ano });
        }

        public async Task<IEnumerable<ParametrosSistema>> ObterParametrosPorTipoEAno(TipoParametroSistema tipo, int ano)
        {
            var query = @"select *
                            from parametros_sistema ps
                           where ano = @ano
                             and tipo = @tipo
                             and ativo";

            return await database.Conexao.QueryAsync<ParametrosSistema>(query.ToString(), new { tipo, ano });
        }

        public async Task<string> ObterNovosTiposUEPorAno(int anoLetivo)
        {
            var query = @"select STRING_AGG(valor, ',') from parametros_sistema ps where ano >= @anoLetivo and tipo = 55";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { anoLetivo });
        }

        public async Task<string> ObterNovasModalidadesAPartirDoAno(int anoLetivo)
        {
            var query = @"select STRING_AGG(valor, ',') from parametros_sistema ps where ano >= @anoLetivo and tipo = @tipo";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { anoLetivo, tipo = TipoParametroSistema.NovasModalidades });
        }

        public async Task<IEnumerable<ParametrosSistema>> ObterParametrosPorAnoAsync(int? ano)
        {
            var query = new StringBuilder(@"select *
                            from parametros_sistema ");

            if (ano.HasValue)
                query.AppendLine("where (ano = @ano or ano is null)");                             
            else query.AppendLine("where ano is null");

            query.AppendLine(" and ativo");

            return await database.Conexao.QueryAsync<ParametrosSistema>(query.ToString(), new { ano });
        }
    }
}