using Dapper;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{

    public static class DapperExtensionMethods
    {
        private static TelemetryClient insightsClient;
        public static void Init(TelemetryClient telemetryClientInjected)
        {
            insightsClient = telemetryClientInjected;
        }
        public static IEnumerable<dynamic> Query(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, param, transaction, buffered, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }

        }
        public static IEnumerable<T> Query<T>(this IDbConnection Connection, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query<T>(Connection, sql, param, transaction, buffered, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }

        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryFirstOrDefaultAsync<T>(cnn, sql, param, transaction, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryFirstOrDefaultAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryFirstOrDefaultAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }

        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }

        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }

        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Query(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await SqlMapper.QueryAsync(cnn, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "QueryAsync", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static int Execute(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = SqlMapper.Execute(cnn, sql, param, transaction, commandTimeout, commandType);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Execute", sql, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Execute", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }

        public static int Execute(this IDbConnection cnn, CommandDefinition command)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                var result = SqlMapper.Execute(cnn, command);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Execute", command.CommandText, inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Execute", $"{command.CommandText} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }

        #region Repositório Base

        public static IEnumerable<TEntity> GetAll<TEntity>(this IDbConnection connection, bool buffered = true) where TEntity : class
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = Dommel.DommelMapper.GetAll<TEntity>(connection, buffered);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "GetAll", nameof(TEntity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "GetAll", $"{nameof(TEntity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }      
        public static object Insert<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                var result = Dommel.DommelMapper.Insert<TEntity>(connection, entity, transaction);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Insert", nameof(entity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Insert", $"{nameof(entity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }

        public static bool Update<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = Dommel.DommelMapper.Update<TEntity>(connection, entity, transaction);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Update", nameof(entity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Update", $"{nameof(entity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static TEntity Get<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                var result = Dommel.DommelMapper.Get<TEntity>(connection, id);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Get", $"{nameof(TEntity)} -> id {id}", inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Get", $"{nameof(TEntity)} -> id {id} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static bool Delete<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                var result = Dommel.DommelMapper.Delete<TEntity>(connection, entity, transaction);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "Delete", nameof(entity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "Delete", $"{nameof(entity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<TEntity> GetAsync<TEntity>(this IDbConnection connection, object id) where TEntity : class
        {

            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await Dommel.DommelMapper.GetAsync<TEntity>(connection, id);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "GetAsync", $"{nameof(TEntity)} -> {id}", inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "GetAsync", $"{nameof(TEntity)} -> {id}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<bool> UpdateAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null)
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                var result = await Dommel.DommelMapper.UpdateAsync<TEntity>(connection, entity, transaction);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "UpdateAsync", nameof(entity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "UpdateAsync", $"{nameof(entity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }
        public static async Task<object> InsertAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            

            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                var result = await Dommel.DommelMapper.InsertAsync<TEntity>(connection, entity, transaction);

                timer.Stop();

                insightsClient.TrackDependency("PostgreSQL", "InsertAsync", nameof(entity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                insightsClient.TrackDependency("PostgreSQL", "InsertAsync", $"{nameof(entity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw ex;
            }
        }


        #endregion

    }
}
