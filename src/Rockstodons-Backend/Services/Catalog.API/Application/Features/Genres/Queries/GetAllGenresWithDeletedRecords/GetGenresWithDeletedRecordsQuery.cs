using Catalog.API.Application.Contracts;
using Catalog.API.Data.Models;

namespace Catalog.API.Application.Features.Genres.Queries.GetAllGenresWithDeletedRecords
{
    public sealed record GetAllGenresWithDeletedRecordsQuery
        : IQuery<List<Genre>>;
}
