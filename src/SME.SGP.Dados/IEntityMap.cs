using System;
using System.Collections.Generic;

namespace SME.SGP.Dados
{
    public interface IEntityMap
    {
        Type EntityType { get; }
        string TableName { get; }
        Dictionary<string, string> ColumnMappings { get; }
        string GetColumnName(string propertyName);
    }
}
