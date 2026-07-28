using System;
using System.Collections.Generic;

namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public abstract class EntityMap<T> : IEntityMap where T : class
    {
        protected Dictionary<string, string> columnMappings = new Dictionary<string, string>();
        protected string tableName;

        public Type EntityType => typeof(T);
        public string TableName => tableName;
        public Dictionary<string, string> ColumnMappings => columnMappings;

        public string GetColumnName(string propertyName)
        {
            if (columnMappings.TryGetValue(propertyName, out var columnName))
                return columnName;
            return propertyName;
        }

        protected void ToTable(string name) => tableName = name;
        protected void Map(string propertyName, string columnName) => columnMappings[propertyName] = columnName;
    }
}
