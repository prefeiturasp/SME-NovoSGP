using Dapper;
using Microsoft.ApplicationInsights;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

//namespace => SME.SGP.Dados.Interceptors
namespace SME.SGP.Dados
{
    public static class DapperExtensionMethods
    {
        private static IServicoTelemetria servicoTelemetria;

        public static void Init(IServicoTelemetria servicoTelemetriaSgp)
        {
            //o ideal seria o servico de telemetria vir por parametro nos metodos
            //pela natureza estatica de methods extension complica a vida de se trabalhar com DI
            //talvez o que ajude para evitar a chamada de services.BuildServiceProvider seja criar um metodo estatico que receba o service provider e nos metodos de chamada
            //use o provedor para montar a interface de telemetria, como iserviceprovider é uma interface daria pra mockar tranquilo em outros casos
            servicoTelemetria = servicoTelemetriaSgp;
        }

        public static IEnumerable<dynamic> Query(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null, string queryName = "query")
        {
            throw new NotImplementedException("Telemetria não implementada para esta função;");
            //remover sempre codigo comentado
        }

        public static IEnumerable<T> Query<T>(this IDbConnection Connection, string sql, object param = null,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null, string queryName = "")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query<T>(Connection, sql, param, transaction, buffered, commandTimeout, commandType),
                "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null,
            string queryName = "")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType),
                "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null,
            string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync<T>(
                async () => await SqlMapper.QueryFirstOrDefaultAsync<T>(cnn, sql, param, transaction, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql,
            Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null,
            string queryName = "Query Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null,
            string queryName = "Query Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            this IDbConnection cnn, string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout,
                    commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDbConnection cnn,
            string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null,
            string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(
            this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<TReturn>>
            QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, string sql,
                Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null,
                IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
                int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static async Task<IEnumerable<TReturn>>
            QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn,
                string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
                object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
                int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());
        }

        public static int Execute(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null,
            string queryName = "Command Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => SqlMapper.Execute(cnn, sql, param, transaction, commandTimeout, commandType), "Postgres",
                $"Command {queryName}", sql, param?.ToString());
        }

        public static int Execute(this IDbConnection cnn, CommandDefinition command,
            string queryName = "Command Postgres")
        {
            return servicoTelemetria.RegistrarComRetorno<int>(() => SqlMapper.Execute(cnn, command), "Postgres",
                $"Command {queryName}", command.ToString());
        }

        #region Repositório Base

        public static IEnumerable<TEntity> GetAll<TEntity>(this IDbConnection connection, bool buffered = true)
            where TEntity : class
        {
            return servicoTelemetria.RegistrarComRetorno(
                () => Dommel.DommelMapper.GetAll<TEntity>(connection, buffered: buffered), "Postgres",
                $"GetAll Entidade ??", "GetAll");
        }

        public static object Insert<TEntity>(this IDbConnection connection, TEntity entity,
            IDbTransaction transaction = null) where TEntity : class
        {
            var entidade = entity?.GetType()?.Name;
            return servicoTelemetria.RegistrarComRetorno(
                () => Dommel.DommelMapper.Insert(connection, entity, transaction), "Postgres",
                $"Insert Entidade {entidade}", "Insert");
        }

        public static bool Update<TEntity>(this IDbConnection connection, TEntity entity,
            IDbTransaction transaction = null)
        {
            var entidade = entity?.GetType()?.Name;
            return servicoTelemetria.RegistrarComRetorno(
                () => Dommel.DommelMapper.Update(connection, entity, transaction), "Postgres",
                $"Update Entidade {entidade}", "Insert");
        }

        public static TEntity Get<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {
            return servicoTelemetria.RegistrarComRetorno(() => Dommel.DommelMapper.Get<TEntity>(connection, id),
                "Postgres", $"Get Entidade ??", "Get");
        }

        public static bool Delete<TEntity>(this IDbConnection connection, TEntity entity,
            IDbTransaction transaction = null, string queryName = "Command Postgres")
        {
            //esse tipo de codigo pode tentar fazer boxing e alocar memoria dependendo do tipo (Struct, Class, Enum) etc
            var entidade = entity?.GetType()?.Name;

            return servicoTelemetria.RegistrarComRetorno(
                () => Dommel.DommelMapper.Delete(connection, entity, transaction), "Postgres",
                $"Get Entidade {entidade}", "Get");
        }

        public static async Task<TEntity> GetAsync<TEntity>(this IDbConnection connection, object id)
            where TEntity : class
        {
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await Dommel.DommelMapper.GetAsync<TEntity>(connection, id), "Postgres",
                $"GetAsync Entidade ??", "GetAsync");
        }

        public static async Task<bool> UpdateAsync<TEntity>(this IDbConnection connection, TEntity entity,
            IDbTransaction transaction = null)
        {
            //talvez usar typeof pra saber o tipo generico da entidade sem fazer boxing e alocar memoria
            var entidade = TypeOf<TEntity>();
            //essas chamadas de delegates vao ficar fazendo alocacao de memoria e ainda vao captura os parametros
            //do metodo, nao seria melhor criar assinaturas genericas que recebem os
            //parametros sem alocar uma funcao
            return await servicoTelemetria.RegistrarComRetornoAsync(connection, entity, transaction,
                async (c, e, t) => await Dommel.DommelMapper.UpdateAsync(c, e, t), "Postgres",
                $"UpdateAsync Entidade {entidade}", "UpdateAsync");
        }

        public static async Task<object> InsertAsync<TEntity>(this IDbConnection connection, TEntity entity,
            IDbTransaction transaction = null) where TEntity : class
        {
            var entidade = entity?.GetType()?.Name;
            return await servicoTelemetria.RegistrarComRetornoAsync(
                async () => await Dommel.DommelMapper.InsertAsync(connection, entity, transaction), "Postgres",
                $"UpdateAsync Entidade {entidade}", "UpdateAsync");
        }

        private static string TypeOf<T>()
        {
            return typeof(T).Name;
        }

        #endregion
    }
}