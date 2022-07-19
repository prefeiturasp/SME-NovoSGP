using Dapper;
using Microsoft.ApplicationInsights;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using System.Text;

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

            //var result = servicoTelemetria.RegistrarAsync( async () => await Task.FromResult(SqlMapper.Query(cnn, sql, param, transaction, buffered, commandTimeout, commandType)), "Postgres", "Query", sql);


            //return default;
            //var inicioOperacao = DateTime.UtcNow;
            //var timer = System.Diagnostics.Stopwatch.StartNew();
            //IEnumerable<dynamic> result = default;
            //try
            //{
            //    var transactionElk = Agent.Tracer.CurrentTransaction;

            //    transactionElk.CaptureSpan("Query", "Postgres", () =>
            //    {
            //        result = SqlMapper.Query(cnn, sql, param, transaction, buffered, commandTimeout, commandType);
            //    });

            //    timer.Stop();

            //    insightsClient?.TrackDependency("PostgreSQL", "Query", sql, inicioOperacao, timer.Elapsed, true);

            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    insightsClient?.TrackDependency("PostgreSQL", "Query", $"{sql} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
            //    throw ex;
            //}

        }

        public static long InsertBulk<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var isList = false;

            var type = typeof(T);

            if (type.IsArray)
            {
                isList = true;
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    isList = true;
                    type = type.GetGenericArguments()[0];
                }
            }

            var name = GetTableName(type);
            var sbColumnList = new StringBuilder(null);
            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type);
            var computedProperties = ComputedPropertiesCache(type);
            var allPropertiesExceptKeyAndComputed = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            var adapter = GetFormatter(connection);

            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                adapter.AppendColumnName(sbColumnList, property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbParameterList.Append(", ");
            }

            int returnVal;
            var wasClosed = connection.State == ConnectionState.Closed;
            if (wasClosed) connection.Open();

            if (!isList)
            {
                returnVal = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(),
                    sbParameterList.ToString(), keyProperties, entityToInsert);
            }
            else
            {
                var cmd = $"insert into {name} ({sbColumnList}) values ({sbParameterList})";
                returnVal = connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
            }
            if (wasClosed) connection.Close();
            return returnVal;
        }
        public static bool UpdateBulk<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (entityToUpdate is IProxy proxy && !proxy.IsDirty)
            {
                return false;
            }
            var type = typeof(T);
            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }
            var keyProperties = KeyPropertiesCache(type).ToList();
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("A entidade deve ter pelo menos uma propriedade [Key] ou [ExplicitKey]");

            var name = GetTableName(type);
            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);

            var allProperties = TypePropertiesCache(type);
            keyProperties.AddRange(explicitKeyProperties);
            var computedProperties = ComputedPropertiesCache(type);
            var nonIdProps = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            var adapter = GetFormatter(connection);

            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                adapter.AppendColumnNameEqualsValue(sb, property.Name);
                if (i < nonIdProps.Count - 1)
                    sb.Append(", ");
            }
            sb.Append(" where ");
            for (var i = 0; i < keyProperties.Count; i++)
            {
                var property = keyProperties[i];
                adapter.AppendColumnNameEqualsValue(sb, property.Name);
                if (i < keyProperties.Count - 1)
                    sb.Append(" and ");
            }
            var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }
        public static bool DeleteBulk<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (entityToDelete == null)
                throw new ArgumentException("Cannot Delete null Object", nameof(entityToDelete));

            var type = typeof(T);

            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }

            var keyProperties = KeyPropertiesCache(type).ToList();
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            var name = GetTableName(type);
            keyProperties.AddRange(explicitKeyProperties);

            var sb = new StringBuilder();
            sb.AppendFormat("delete from {0} where ", name);

            var adapter = GetFormatter(connection);

            for (var i = 0; i < keyProperties.Count; i++)
            {
                var property = keyProperties[i];
                adapter.AppendColumnNameEqualsValue(sb, property.Name);
                if (i < keyProperties.Count - 1)
                    sb.Append(" and ");
            }
            var deleted = connection.Execute(sb.ToString(), entityToDelete, transaction, commandTimeout);
            return deleted > 0;
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

        #region Repositório Base

        public static IEnumerable<TEntity> GetAll<TEntity>(this IDbConnection connection, bool buffered = true) where TEntity : class
        {
            //Descobrir como obter a classe;;
            //var entidade = this. TEntity?.GetType()?.Name;

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
            //var entidade = entity?.GetType()?.Name;

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
            //var entidade = entity?.GetType()?.Name;

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

        public interface IProxy //must be kept public
        {
            /// <summary>
            /// Whether the object has been changed.
            /// </summary>
            bool IsDirty { get; set; }
        }
        public delegate string TableNameMapperDelegate(Type type);
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ComputedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        [AttributeUsage(AttributeTargets.Property)]
        public class KeyAttribute : Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Property)]
        public class ExplicitKeyAttribute : Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Property)]
        public class WriteAttribute : Attribute
        {
            /// <summary>
            /// Specifies whether a field is writable in the database.
            /// </summary>
            /// <param name="write">Whether a field is writable in the database.</param>
            public WriteAttribute(bool write)
            {
                Write = write;
            }

            /// <summary>
            /// Whether a field is writable in the database.
            /// </summary>
            public bool Write { get; }
        }
        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1) return true;

            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }
        private static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(IsWriteable).ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }
        private static List<PropertyInfo> KeyPropertiesCache(Type type)
        {
            if (KeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            if (keyProperties.Count == 0)
            {
                var idProp = allProperties.Find(p => string.Equals(p.Name, "id", StringComparison.CurrentCultureIgnoreCase));
                if (idProp != null && !idProp.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute))
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }
        private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
        {
            if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();

            ExplicitKeyProperties[type.TypeHandle] = explicitKeyProperties;
            return explicitKeyProperties;
        }
        [AttributeUsage(AttributeTargets.Class)]
        public class TableAttribute : Attribute
        {
            /// <summary>
            /// Creates a table mapping to a specific name for Dapper.Contrib commands
            /// </summary>
            /// <param name="tableName">The name of this table in the database.</param>
            public TableAttribute(string tableName)
            {
                Name = tableName;
            }

            /// <summary>
            /// The name of the table in the database
            /// </summary>
            public string Name { get; set; }
        }
        public static TableNameMapperDelegate TableNameMapper;
        public partial interface ISqlAdapter
        {
            int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert);
            void AppendColumnName(StringBuilder sb, string columnName);
            void AppendColumnNameEqualsValue(StringBuilder sb, string columnName);
        }
        public partial class PostgresAdapter : ISqlAdapter
        {
            public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

                var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
                if (propertyInfos.Length == 0)
                {
                    sb.Append(" RETURNING *");
                }
                else
                {
                    sb.Append(" RETURNING ");
                    var first = true;
                    foreach (var property in propertyInfos)
                    {
                        if (!first)
                            sb.Append(", ");
                        first = false;
                        sb.Append(property.Name);
                    }
                }

                var results = connection.Query(sb.ToString(), entityToInsert, transaction, commandTimeout: commandTimeout).ToList();

                var id = 0;
                foreach (var p in propertyInfos)
                {
                    var value = ((IDictionary<string, object>)results[0])[p.Name.ToLower()];
                    p.SetValue(entityToInsert, value, null);
                    if (id == 0)
                        id = Convert.ToInt32(value);
                }
                return id;
            }

            public void AppendColumnName(StringBuilder sb, string columnName)
            {
                sb.AppendFormat("\"{0}\"", columnName);
            }
            public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
            {
                sb.AppendFormat("\"{0}\" = @{1}", columnName, columnName);
            }
        }
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary
            = new Dictionary<string, ISqlAdapter>(1)
            {
                ["npgsqlconnection"] = new PostgresAdapter()
            };
        public delegate string GetDatabaseTypeDelegate(IDbConnection connection);
        [AttributeUsage(AttributeTargets.Property)]
        public class ComputedAttribute : Attribute
        {
        }
        public static GetDatabaseTypeDelegate GetDatabaseType;
        private static readonly ISqlAdapter DefaultAdapter = new PostgresAdapter();
        private static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            var name = GetDatabaseType?.Invoke(connection).ToLower()
                       ?? connection.GetType().Name.ToLower();

            return AdapterDictionary.TryGetValue(name, out var adapter)
                ? adapter
                : DefaultAdapter;
        }
        private static List<PropertyInfo> ComputedPropertiesCache(Type type)
        {
            if (ComputedProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var computedProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            ComputedProperties[type.TypeHandle] = computedProperties;
            return computedProperties;
        }
        private static string GetTableName(Type type)
        {
            if (TypeTableName.TryGetValue(type.TypeHandle, out string name)) return name;

            if (TableNameMapper != null)
            {
                name = TableNameMapper(type);
            }
            else
            {
                var tableAttrName =
                    type.GetCustomAttribute<TableAttribute>(false)?.Name
                    ?? (type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic)?.Name;

                if (tableAttrName != null)
                {
                    name = tableAttrName;
                }
                else
                {
                    name = type.Name + "s";
                    if (type.IsInterface && name.StartsWith("I"))
                        name = name.Substring(1);
                }
            }

            TypeTableName[type.TypeHandle] = name;
            return name;
        }
    }
}
