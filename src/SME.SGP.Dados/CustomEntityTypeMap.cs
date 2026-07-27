using Dapper;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SME.SGP.Dados
{
    public class CustomEntityTypeMap : SqlMapper.ITypeMap
    {
        private readonly Type type;
        private readonly IEntityMap entityMap;
        private readonly Dictionary<string, PropertyInfo> propertyCache;

        public CustomEntityTypeMap(Type type, IEntityMap entityMap)
        {
            this.type = type;
            this.entityMap = entityMap;
            this.propertyCache = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in type.GetProperties())
            {
                propertyCache[prop.Name] = prop;
                propertyCache[entityMap.GetColumnName(prop.Name)] = prop;
            }
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types) => null;

        public ConstructorInfo FindExplicitConstructor() => null;

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName) => null;

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            PropertyInfo prop = null;

            foreach (var kvp in entityMap.ColumnMappings)
            {
                if (kvp.Value.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    propertyCache.TryGetValue(kvp.Key, out prop);
                    break;
                }
            }

            if (prop == null)
                propertyCache.TryGetValue(columnName, out prop);

            return prop != null ? new SimpleMemberMap(columnName, prop) : null;
        }
    }
}
