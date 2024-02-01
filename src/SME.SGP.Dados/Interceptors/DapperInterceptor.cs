using Dapper;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{

    public static class DapperExtensionMethods
    {
        private static IServicoTelemetria servicoTelemetria;

        public static void Init(IServicoTelemetria servicoTelemetriaSgp)
        {
            servicoTelemetria = servicoTelemetriaSgp;
        }
        public static IEnumerable<dynamic> Query(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, string queryName = "query")
        {
            throw new NotImplementedException("Telemtria não implementada para esta função;");
        }
        public static IEnumerable<T> Query<T>(this IDbConnection Connection, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, string queryName = "")
        {
            var result = servicoTelemetria.RegistrarComRetorno<T>(() => SqlMapper.Query<T>(Connection, sql, param, transaction, buffered, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string queryName = "")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<T>(async () => await SqlMapper.QueryAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;

        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<T>(async () => await SqlMapper.QueryFirstOrDefaultAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;

        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<TReturn>(() => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<TReturn>(() => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<TReturn>(() => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<TReturn>(() => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<TReturn>(() => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<TReturn>(() => SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TReturn>(async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TReturn>(async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TReturn>(async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TReturn>(async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TReturn>(async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, string queryName = "Query Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TReturn>(async () => await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType), "Postgres", $"Query {queryName}", sql, param?.ToString());

            return result;
        }
        public static int Execute(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string queryName = "Command Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<int>(() => SqlMapper.Execute(cnn, sql, param, transaction, commandTimeout, commandType), "Postgres", $"Command {queryName}", sql, param?.ToString());

            return result;
        }

        public static int Execute(this IDbConnection cnn, CommandDefinition command, string queryName = "Command Postgres")
        {
            var result = servicoTelemetria.RegistrarComRetorno<int>(() => SqlMapper.Execute(cnn, command), "Postgres", $"Command {queryName}", command.ToString());

            return result;
        }

        public static async Task<int> ExecuteAsync(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string queryName = "Command Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<int>(async () => await SqlMapper.ExecuteAsync(cnn, sql, param, transaction, commandTimeout, commandType), "Postgres", $"Command {queryName}", sql, param?.ToString());
            
            return result;
        }

        public static async Task<int> ExecuteAsync(this IDbConnection cnn, CommandDefinition command, string queryName = "Command Postgres")
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<int>(async () => await SqlMapper.ExecuteAsync(cnn, command), "Postgres", $"Command {queryName}", command.ToString());

            return result;
        }

        #region Repositório Base

        public static IEnumerable<TEntity> GetAll<TEntity>(this IDbConnection connection, bool buffered = true) where TEntity : class
        {
            var result = servicoTelemetria.RegistrarComRetorno<TEntity>(() => Dommel.DommelMapper.GetAll<TEntity>(connection, buffered: buffered), "Postgres", $"GetAll Entidade ??", "GetAll");

            return result;
        }
        public static object Insert<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            var entidade = entity?.GetType()?.Name;

            var result = servicoTelemetria.RegistrarComRetorno<TEntity>(() => Dommel.DommelMapper.Insert<TEntity>(connection, entity, transaction), "Postgres", $"Insert Entidade {entidade}", "Insert");

            return result;
        }

        public static bool Update<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            var entidade = entity?.GetType()?.Name;

            var result = servicoTelemetria.RegistrarComRetorno<TEntity>(() => Dommel.DommelMapper.Update<TEntity>(connection, entity, transaction), "Postgres", $"Update Entidade {entidade}", "Insert");

            return result;
        }
        public static TEntity Get<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {
            var result = servicoTelemetria.RegistrarComRetorno<TEntity>(() => Dommel.DommelMapper.Get<TEntity>(connection, id), "Postgres", $"Get Entidade ??", "Get");

            return result;
        }
        public static bool Delete<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null, string queryName = "Command Postgres")
        {
            var entidade = entity?.GetType()?.Name;

            var result = servicoTelemetria.RegistrarComRetorno<TEntity>(() => Dommel.DommelMapper.Delete<TEntity>(connection, entity, transaction), "Postgres", $"Get Entidade {entidade}", "Get");

            return result;
        }
        public static async Task<TEntity> GetAsync<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<TEntity>(async () => await Dommel.DommelMapper.GetAsync<TEntity>(connection, id), "Postgres", $"GetAsync Entidade ??", "GetAsync");

            return result;

        }
        public static async Task<bool> UpdateAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            var entidade = entity?.GetType()?.Name;

            var result = await servicoTelemetria.RegistrarComRetornoAsync<TEntity>(async () => await Dommel.DommelMapper.UpdateAsync<TEntity>(connection, entity, transaction), "Postgres", $"UpdateAsync Entidade {entidade}", "UpdateAsync");

            return result;
        }
        public static async Task<object> InsertAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            var entidade = entity?.GetType()?.Name;

            var result = await servicoTelemetria.RegistrarComRetornoAsync<TEntity>(async () => await Dommel.DommelMapper.InsertAsync<TEntity>(connection, entity, transaction), "Postgres", $"UpdateAsync Entidade {entidade}", "UpdateAsync");

            return result;

        }
        #endregion
    }
}
