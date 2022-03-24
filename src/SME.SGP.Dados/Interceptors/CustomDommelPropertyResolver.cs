using Dapper.FluentMap;
using Dapper.FluentMap.Dommel.Resolvers;
using Dapper.FluentMap.Mapping;
using Dommel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Interceptors
{
    /// <summary>
    /// Resolve o crud das propriedades mantendo apenas as propriedade que estão mapeadas.
    /// </summary>
    public class CustomDommelPropertyResolver : DommelPropertyResolver
    {
        public override IEnumerable<ColumnPropertyInfo> ResolveProperties(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var listaPropertyMaps = entityMap.PropertyMaps.Where(prop => !prop.Ignored);

                foreach (var propertyMap in listaPropertyMaps)
                {
                    yield return new ColumnPropertyInfo(propertyMap.PropertyInfo);
                }
            }
            else
            {
                foreach (var property in ResolveProperties(type))
                {
                    yield return property;
                }
            }
        }
    }
}
