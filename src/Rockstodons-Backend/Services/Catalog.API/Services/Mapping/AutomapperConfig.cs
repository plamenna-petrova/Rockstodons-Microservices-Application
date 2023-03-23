using AutoMapper;
using AutoMapper.Internal;
using System.Reflection;

namespace Catalog.API.Services.Mapping
{
    public class AutomapperConfig
    {
        private static bool isInitialized;

        public static IMapper MapperInstance { get; set; }

        public static void RegisterMappings(params Assembly[] assemblies)
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            var publicExportedTypes = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();
            var mapperConfigurationExpression = new MapperConfigurationExpression();

            mapperConfigurationExpression.CreateProfile("ReflectionProfile", configuration =>
            {
                foreach (var sourceMapping in GetSourceMappings(publicExportedTypes))
                {
                    mapperConfigurationExpression.CreateMap(sourceMapping.Source, sourceMapping.Destination);
                }

                foreach (var destinationMapping in GetDestinationMappings(publicExportedTypes))
                {
                    mapperConfigurationExpression.CreateMap(destinationMapping.Source, destinationMapping.Destination);
                }

                foreach (var customMapping in GetCustomMappings(publicExportedTypes))
                {
                    customMapping.CreateMappings(mapperConfigurationExpression);
                }
            });

            MapperInstance = new Mapper(new MapperConfiguration(mapperConfigurationExpression));
        }

        private static IEnumerable<TypesMap> GetSourceMappings(IEnumerable<Type> types)
        {
            var sourceMappings = from t in types
                                 from i in t.GetTypeInfo().GetInterfaces()
                                 where i.GetTypeInfo().IsGenericType &&
                                       i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                       !t.GetTypeInfo().IsAbstract &&
                                       !t.GetTypeInfo().IsInterface
                                 select new TypesMap
                                 {
                                     Source = i.GetTypeInfo().GetGenericArguments()[0],
                                     Destination = t,
                                 };

            return sourceMappings;
        }

        private static IEnumerable<TypesMap> GetDestinationMappings(IEnumerable<Type> types)
        {
            var destinationMappings = from t in types
                                      from i in t.GetTypeInfo().GetInterfaces()
                                      where i.GetTypeInfo().IsGenericType &&
                                            i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                                            !t.GetTypeInfo().IsAbstract &&
                                            !t.GetTypeInfo().IsInterface
                                      select new TypesMap
                                      {
                                          Source = i.GetType().GetGenericArguments()[0],
                                          Destination = t,
                                      };

            return destinationMappings;
        }

        private static IEnumerable<ICustomMappings> GetCustomMappings(IEnumerable<Type> types)
        {
            var customMappings = from t in types
                                 from i in t.GetTypeInfo().GetInterfaces()
                                 where typeof(ICustomMappings).GetTypeInfo().IsAssignableFrom(t) &&
                                 !t.GetTypeInfo().IsAbstract && !t.GetType().IsInterface
                                 select (ICustomMappings) Activator.CreateInstance(t)!;

            return customMappings;
        }

        private class TypesMap
        {
            public Type Source { get; set; }

            public Type Destination { get; set; }
        }
    }
}
