﻿using Dapper;
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

        public async Task<IEnumerable<KeyValuePair<string, string>>> ObterChaveEValorPorTipo(TipoParametroSistema tipo)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select nome as Key, valor as Value");
            query.AppendLine("from parametros_sistema");
            query.AppendLine("where ativo and tipo = @tipo");

            var resultado = await database.Conexao.QueryAsync<KeyValuePair<string, string>>(query.ToString(), new { tipo });

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

            return await database.Conexao.QueryFirstAsync<string>(query, new { tipoParametroSistema });
        }

        public async Task<T> ObterValorUnicoPorTipo<T>(TipoParametroSistema tipoParametroSistema)
        {

            var query = @"select valor
                          from parametros_sistema
                         where tipo = @tipoParametroSistema and ativo";

            return await database.Conexao.QueryFirstAsync<T>(query, new { tipoParametroSistema });
        }
    }
}