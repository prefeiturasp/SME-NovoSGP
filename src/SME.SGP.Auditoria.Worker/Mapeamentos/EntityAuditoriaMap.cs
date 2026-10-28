using System;
using System.Collections.Generic;

namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public abstract class EntityAuditoriaMap<T> : IEntityAuditoriaMap
        where T : class
    {
        private readonly Dictionary<string, string> columns =
            new(StringComparer.OrdinalIgnoreCase);

        public Type EntityType => typeof(T);

        public string TableName { get; private set; }

        public Dictionary<string, string> ColumnMappings =>
            columns;

        public IReadOnlyDictionary<string, string> Columns =>
            columns;

        public string GetColumnName(string propertyName)
        {
            return columns.TryGetValue(
                propertyName,
                out var columnName)
                    ? columnName
                    : propertyName;
        }

        protected void ToTable(string tableName)
        {
            TableName = tableName;
        }

        protected void Map(
            string propertyName,
            string columnName)
        {
            columns[propertyName] = columnName;
        }
    }
}