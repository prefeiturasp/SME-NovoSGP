using Dapper;
using System;
using System.Reflection;

namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public class SimpleMemberMap : SqlMapper.IMemberMap
    {
        public SimpleMemberMap(string columnName, PropertyInfo property)
        {
            ColumnName = columnName;
            Property = property;
        }

        public string ColumnName { get; }
        public Type MemberType => Property.PropertyType;
        public PropertyInfo Property { get; }
        public FieldInfo Field => null;
        public ParameterInfo Parameter => null;
    }
}
