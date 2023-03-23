using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;

namespace Catalog.API.Services.Mapping
{
    public static class QueryableMappingExtensions
    {
        public static IQueryable<TDestination> MapTo<TDestination>(
            this IQueryable source, 
            params Expression<Func<TDestination, object>>[] membersToExpand
        )
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(AutomapperConfig.MapperInstance.ConfigurationProvider, null, membersToExpand);
        }

        public static IQueryable<TDestination> MapTo<TDestination>(this IQueryable source, object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(AutomapperConfig.MapperInstance.ConfigurationProvider, parameters);
        }
    }
}
