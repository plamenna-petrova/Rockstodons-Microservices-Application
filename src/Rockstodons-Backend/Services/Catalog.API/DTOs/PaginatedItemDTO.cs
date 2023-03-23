namespace Catalog.API.DTOs
{
    public class PaginatedItemDTO<TEntity> where TEntity : class
    {
        public int PageIndex { get; private set; } 

        public int PageSize { get; private set; }

        public long PagesCount { get; private set; }

        public IEnumerable<TEntity> Data { get; private set;}

        public PaginatedItemDTO(int pageIndex, int pageSize, long pagesCount, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            PagesCount = pagesCount;
            Data = data;
        }
    }
}
