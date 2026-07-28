using System;
using System.Collections.Generic;

namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public interface IEntityMap
    {
        Type EntityType { get; }
        string TableName { get; }
        Dictionary<string, string> ColumnMappings { get; }
        string GetColumnName(string propertyName);
    }
}
