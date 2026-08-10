using System;
using System.Collections.Concurrent;
using System.Linq;

namespace SME.SGP.Dados
{
    public static class MapRegistry
    {
        private static readonly object SyncRoot = new();

        private static readonly ConcurrentDictionary<Type, IEntityMap>
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

                var assembly = typeof(MapRegistry).Assembly;

                var mapTypes = assembly
                    .GetTypes()
                    .Where(type =>
                        !type.IsAbstract &&
                        typeof(SME.SGP.Dados.IEntityMap).IsAssignableFrom(type))
                    .ToList();

                foreach (var mapType in mapTypes)
                {
                    var map = (SME.SGP.Dados.IEntityMap)Activator.CreateInstance(mapType);

                    RegisteredMaps[map.EntityType] = map;
                }

                initialized = true;
            }
        }

        public static IEntityMap GetMap(Type entityType)
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

        public static IEntityMap Get<T>()
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