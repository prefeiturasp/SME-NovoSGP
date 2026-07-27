using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;

namespace SME.SGP.Dados
{
    public static class MapRegistry
    {
        private static Dictionary<Type, IEntityMap> registeredMaps = new Dictionary<Type, IEntityMap>();
        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized)
                return;

            var entityMapOpenType = typeof(EntityMap<>);
            var assembly = Assembly.GetExecutingAssembly();

            var mapTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && ImplementsEntityMap(t, entityMapOpenType))
                .ToList();

            foreach (var mapTypeClass in mapTypes)
            {
                try
                {
                    var mapInstance = (IEntityMap)Activator.CreateInstance(mapTypeClass);
                    registeredMaps[mapInstance.EntityType] = mapInstance;

                    var method = typeof(SqlMapper)
                        .GetMethod("SetTypeMap", BindingFlags.Public | BindingFlags.Static);

                    var customMapInstance = new CustomEntityTypeMap(mapInstance.EntityType, mapInstance);

                    method?.Invoke(null, new object[] { mapInstance.EntityType, customMapInstance });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao registrar map para {mapTypeClass.Name}: {ex.Message}");
                }
            }

            initialized = true;
        }

        private static bool ImplementsEntityMap(Type type, Type entityMapOpenType)
        {
            var current = type.BaseType;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == entityMapOpenType)
                    return true;
                current = current.BaseType;
            }
            return false;
        }

        public static IEntityMap GetMap(Type entityType)
        {
            registeredMaps.TryGetValue(entityType, out var map);
            return map;
        }

        public static string GetTableName(Type entityType)
        {
            var map = GetMap(entityType);
            return map?.TableName ?? entityType.Name.ToLower();
        }

        public static string GetColumnName(Type entityType, string propertyName)
        {
            var map = GetMap(entityType);
            return map?.GetColumnName(propertyName) ?? propertyName;
        }
    }
}