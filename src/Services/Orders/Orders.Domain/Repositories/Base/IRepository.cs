using Orders.Domain.Entities;
using System.Linq.Expressions;

namespace Orders.Domain.Repositories.Base;
public interface IRepository<TypeEntity> where TypeEntity : EntityBase {
    Task<IEnumerable<TypeEntity>> GetAllAsync();
    Task<IEnumerable<TypeEntity>> GetAsync(Expression<Func<TypeEntity, Boolean>> predicate = null,
        Func<IQueryable<TypeEntity>, IOrderedQueryable<TypeEntity>> orderBy = null,
        String includeString = null,
        Boolean disableTracking = true);
    Task<TypeEntity> GetByIdAsync(Guid id);
    Task<TypeEntity> GetByIdAsync(TypeEntity typeEntity);
    Task<TypeEntity> AddAsync(TypeEntity typeEntity);
    Task UpdateAsync(TypeEntity typeEntity);
    Task DeleteAsync(TypeEntity typeEntity);
}