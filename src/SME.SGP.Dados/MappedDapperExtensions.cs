using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    [ExcludeFromCodeCoverage]
    public static class MappedDapperExtensions
    {
        private const string WhereClause = "WHERE ";
        private const string IdParameter = " = @Id";
        private const string SqlStatementTerminator = ";";

        [SuppressMessage("Security", "S2077:Formatting SQL queries is security-sensitive")]
        public static long InsertMapped<T>(
            this IDbConnection connection,
            T entity,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var properties =
                GetInsertProperties<T>(map);

            if (properties.Length == 0)
            {
                throw new InvalidOperationException(
                    $"A entidade '{typeof(T).FullName}' " +
                    "não possui propriedades para inserção.");
            }

            var columns = string.Join(
                ", ",
                properties.Select(property =>
                    QuoteIdentifier(
                        map.GetColumnName(property.Name))));

            var parameters = string.Join(
                ", ",
                properties.Select(property =>
                    "@" + property.Name));

            var sql = 
                "INSERT INTO " +
                QuoteIdentifier(map.TableName) +
                " (" + columns + ")" +
                " VALUES (" + parameters + ")" +
                " RETURNING " +
                QuoteIdentifier(map.GetColumnName("Id")) +
                ";";

            var id = connection.ExecuteScalar<long>(sql, entity, transaction); 
            SetId(entity, id);

            return id;
        }

        [SuppressMessage("Security", "S2077:Formatting SQL queries is security-sensitive")]
        public static async Task<long> InsertMappedAsync<T>(
            this IDbConnection connection,
            T entity,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var properties =
                GetInsertProperties<T>(map);

            if (properties.Length == 0)
            {
                throw new InvalidOperationException(
                    $"A entidade '{typeof(T).FullName}' " +
                    "não possui propriedades para inserção.");
            }

            var columns = string.Join(
                ", ",
                properties.Select(property =>
                    QuoteIdentifier(
                        map.GetColumnName(property.Name))));

            var parameters = string.Join(
                ", ",
                properties.Select(property =>
                    "@" + property.Name));

            var sql =
                "INSERT INTO " +
                QuoteIdentifier(map.TableName) +
                " (" + columns + ")" +
                " VALUES (" + parameters + ")" +
                " RETURNING " +
                QuoteIdentifier(map.GetColumnName("Id")) +
                ";";

            var id = await connection.ExecuteScalarAsync<long>(sql, entity, transaction);

            SetId(entity, id);

            return id;
        }

        public static int UpdateMapped<T>(
            this IDbConnection connection,
            T entity,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var idProperty = GetIdProperty<T>();

            var properties =
                GetUpdateProperties<T>(map);

            if (properties.Length == 0)
            {
                throw new InvalidOperationException(
                    $"A entidade '{typeof(T).FullName}' " +
                    "não possui propriedades para atualização.");
            }

            var assignments = string.Join(
                ", ",
                properties.Select(property =>
                    QuoteIdentifier(
                        map.GetColumnName(property.Name)) +
                    " = @" +
                    property.Name));

            var sql =
                "UPDATE " +
                QuoteIdentifier(map.TableName) +
                " SET " +
                assignments +
                " " +
                WhereClause +
                QuoteIdentifier(
                    map.GetColumnName(idProperty.Name)) +
                IdParameter + SqlStatementTerminator;

            return connection.Execute(
                sql,
                entity,
                transaction);
        }

        public static async Task<int> UpdateMappedAsync<T>(
            this IDbConnection connection,
            T entity,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var idProperty = GetIdProperty<T>();

            var properties =
                GetUpdateProperties<T>(map);

            if (properties.Length == 0)
            {
                throw new InvalidOperationException(
                    $"A entidade '{typeof(T).FullName}' " +
                    "não possui propriedades para atualização.");
            }

            var assignments = string.Join(
                ", ",
                properties.Select(property =>
                    QuoteIdentifier(
                        map.GetColumnName(property.Name)) +
                    " = @" +
                    property.Name));

            var sql =
                "UPDATE " +
                QuoteIdentifier(map.TableName) +
                " SET " +
                assignments +
                " " +
                WhereClause +
                QuoteIdentifier(
                    map.GetColumnName(idProperty.Name)) +
              IdParameter +
              SqlStatementTerminator;

            return await connection.ExecuteAsync(
                sql,
                entity,
                transaction);
        }

        public static IEnumerable<T> GetAllMapped<T>(
            this IDbConnection connection,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var sql = BuildSelectSql<T>(
                map,
                null);

            return connection.Query<T>(
                sql,
                transaction: transaction);
        }

        public static async Task<IEnumerable<T>>
            GetAllMappedAsync<T>(
                this IDbConnection connection,
                IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var sql = BuildSelectSql<T>(
                map,
                null);

            return await connection.QueryAsync<T>(
                sql,
                transaction: transaction);
        }

        public static T GetMapped<T>(
            this IDbConnection connection,
            object id,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var idProperty = GetIdProperty<T>();

            var sql = BuildSelectSql<T>(
                map,
                 WhereClause +
                QuoteIdentifier(
                    map.GetColumnName(idProperty.Name)) +
               IdParameter);

            return connection.QuerySingleOrDefault<T>(
                sql,
                new { Id = id },
                transaction);
        }

        public static async Task<T>
            GetMappedAsync<T>(
                this IDbConnection connection,
                object id,
                IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var idProperty = GetIdProperty<T>();

            var sql = BuildSelectSql<T>(
                map,
                WhereClause +
                QuoteIdentifier(
                    map.GetColumnName(idProperty.Name)) +
                IdParameter);

            return await connection.QuerySingleOrDefaultAsync<T>(
                sql,
                new { Id = id },
                transaction);
        }

        public static int DeleteMapped<T>(
            this IDbConnection connection,
            T entity,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var idProperty = GetIdProperty<T>();

            var sql =
                "DELETE FROM " +
                QuoteIdentifier(map.TableName) +
                 WhereClause +
                QuoteIdentifier(
                    map.GetColumnName(idProperty.Name)) +
                IdParameter +
                SqlStatementTerminator;

            return connection.Execute(
                sql,
                new
                {
                    Id = idProperty.GetValue(entity)
                },
                transaction);
        }

        public static async Task<int> DeleteMappedAsync<T>(
            this IDbConnection connection,
            T entity,
            IDbTransaction transaction = null)
            where T : class
        {
            var map = MapRegistry.GetMap(typeof(T));

            ValidateMap<T>(map);

            var idProperty = GetIdProperty<T>();

            var sql =
                "DELETE FROM " +
                QuoteIdentifier(map.TableName) +
                " " +
                WhereClause +
                QuoteIdentifier(
                    map.GetColumnName(idProperty.Name)) +
                IdParameter +
                SqlStatementTerminator;

            return await connection.ExecuteAsync(
                sql,
                new
                {
                    Id = idProperty.GetValue(entity)
                },
                transaction);
        }

        private static string BuildSelectSql<T>(
            IEntityMap map,
            string whereClause)
            where T : class
        {
            var properties =
                GetSelectProperties<T>(map);

            if (properties.Length == 0)
            {
                throw new InvalidOperationException(
                    $"A entidade '{typeof(T).FullName}' " +
                    "não possui propriedades mapeadas.");
            }

            var columns = string.Join(
                ", ",
                properties.Select(property =>
                    QuoteIdentifier(
                        map.GetColumnName(property.Name)) +
                    " AS " +
                    QuoteIdentifier(property.Name)));

            var sql =
                "SELECT " +
                columns +
                " FROM " +
                QuoteIdentifier(map.TableName);

            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                sql += " " + whereClause;
            }

            sql += ";";

            return sql;
        }

        private static PropertyInfo[] GetInsertProperties<T>(
            IEntityMap map)
            where T : class
        {
            return typeof(T)
                .GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance)
                .Where(property =>
                    property.CanRead &&
                    !IsIdProperty(property) &&
                    map.ColumnMappings.ContainsKey(
                        property.Name))
                .ToArray();
        }

        private static PropertyInfo[] GetUpdateProperties<T>(
            IEntityMap map)
            where T : class
        {
            return typeof(T)
                .GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance)
                .Where(property =>
                    property.CanRead &&
                    property.CanWrite &&
                    !IsIdProperty(property) &&
                    map.ColumnMappings.ContainsKey(
                        property.Name))
                .ToArray();
        }

        private static PropertyInfo[] GetSelectProperties<T>(
            IEntityMap map)
            where T : class
        {
            return typeof(T)
                .GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance)
                .Where(property =>
                    property.CanWrite &&
                    map.ColumnMappings.ContainsKey(
                        property.Name))
                .ToArray();
        }

        private static PropertyInfo GetIdProperty<T>()
            where T : class
        {
            var idProperty = typeof(T).GetProperty(
                "Id",
                BindingFlags.Public |
                BindingFlags.Instance);

            if (idProperty == null)
            {
                throw new InvalidOperationException(
                    $"A entidade '{typeof(T).FullName}' " +
                    "não possui a propriedade Id.");
            }

            return idProperty;
        }

        private static bool IsIdProperty(
            PropertyInfo property)
        {
            return property.Name.Equals(
                "Id",
                StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidateMap<T>(
            IEntityMap map)
            where T : class
        {
            if (map == null)
            {
                throw new InvalidOperationException(
                    $"Mapa não encontrado para a entidade " +
                    $"'{typeof(T).FullName}'.");
            }

            if (string.IsNullOrWhiteSpace(
                    map.TableName))
            {
                throw new InvalidOperationException(
                    $"O mapa da entidade '{typeof(T).FullName}' " +
                    "não possui tabela configurada.");
            }

            if (map.ColumnMappings == null ||
                map.ColumnMappings.Count == 0)
            {
                throw new InvalidOperationException(
                    $"O mapa da entidade '{typeof(T).FullName}' " +
                    "não possui colunas configuradas.");
            }
        }

        private static void SetId<T>(
            T entity,
            long id)
            where T : class
        {
            var idProperty = typeof(T).GetProperty(
                "Id",
                BindingFlags.Public |
                BindingFlags.Instance);

            if (idProperty?.CanWrite == true &&
                idProperty.PropertyType == typeof(long))
            {
                idProperty.SetValue(entity, id);
            }
        }

        private static string QuoteIdentifier(
            string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(
                    "Identificador SQL inválido.",
                    nameof(identifier));
            }

            return "\"" +
                   identifier.Replace("\"", "\"\"") +
                   "\"";
        }
    }
}