using AutoMapper;

namespace Catalog.API.Services.Mapping
{
    public interface ICustomMappings
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
