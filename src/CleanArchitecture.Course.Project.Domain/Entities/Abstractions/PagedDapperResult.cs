namespace CleanArchitecture.Course.Project.Domain.Entities.Abstractions
{
    public class PagedDapperResult<T>
    {

        public PagedDapperResult(int totalCount, int pageNumber = 1, int pageSize = 10)
        {
            var mod = totalCount % pageSize;
            var totalPages = (totalCount / pageSize) + (mod == 0 ? 0 : 1);
            
            TotalPages = totalPages;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public IEnumerable<T>? Items { get; set; }
        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}