namespace Catalog.API.Services.Services.Data.Interfaces
{
    public interface IGenresService
    {
        Task<List<T>> GetAllGenres<T>();
    }
}
