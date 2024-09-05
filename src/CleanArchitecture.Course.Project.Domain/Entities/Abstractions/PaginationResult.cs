namespace CleanArchitecture.Course.Project.Domain.Entities.Abstractions
{
    public class PaginationResult<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int TotalByPage { get; set; }
        public IReadOnlyList<TEntity>? Data { get; set; }
    }
}