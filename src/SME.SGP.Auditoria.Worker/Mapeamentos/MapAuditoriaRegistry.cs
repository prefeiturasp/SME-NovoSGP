using System;
using System.Collections.Concurrent;
using System.Linq;

namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public static class MapAuditoriaRegistry
    {
        private static readonly object SyncRoot = new();

        private static readonly ConcurrentDictionary<Type, IEntityAuditoriaMap>
            RegisteredMaps = new();

        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;

            lock (SyncRoot)
            {
                if (initialized)
                    return;

                var assembly = typeof(MapAuditoriaRegistry).Assembly;

                var mapTypes = assembly
                    .GetTypes()
                    .Where(type =>
                        !type.IsAbstract &&
                        typeof(IEntityAuditoriaMap).IsAssignableFrom(type))
                    .ToList();

                foreach (var mapType in mapTypes)
                {
                    var map = (IEntityAuditoriaMap)Activator.CreateInstance(mapType);

                    RegisteredMaps[map.EntityType] = map;
                }

                initialized = true;
            }
        }

        public static IEntityAuditoriaMap GetMap(Type entityType)
        {
            Initialize();

            if (!RegisteredMaps.TryGetValue(
                    entityType,
                    out var map))
            {
                throw new InvalidOperationException(
                    $"Mapa não encontrado para a entidade '{entityType.FullName}'.");
            }

            return map;
        }

        public static IEntityAuditoriaMap Get<T>()
            where T : class
        {
            return GetMap(typeof(T));
        }

        public static string GetTableName(Type entityType)
        {
            var map = GetMap(entityType);

            if (string.IsNullOrWhiteSpace(map.TableName))
            {
                throw new InvalidOperationException(
                    $"O mapa da entidade '{entityType.FullName}' não possui tabela.");
            }

            return map.TableName;
        }

        public static string GetColumnName(
            Type entityType,
            string propertyName)
        {
            return GetMap(entityType)
                .GetColumnName(propertyName);
        }
    }
}
