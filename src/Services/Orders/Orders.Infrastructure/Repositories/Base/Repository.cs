using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Repositories.Base;
using Orders.Infrastructure.Data;
using System.Linq.Expressions;

namespace Orders.Infrastructure.Repositories.Base;
public class Repository<TypeEntity> : IRepository<TypeEntity> where TypeEntity : EntityBase {
    protected readonly OrderContext _context;
    public Repository(OrderContext context) {
        _context = context;
    }
    public async Task<TypeEntity> AddAsync(TypeEntity typeEntity) {
        await _context.Set<TypeEntity>().AddAsync(typeEntity);
        await _context.SaveChangesAsync();
        return typeEntity;
    }

    public async Task DeleteAsync(TypeEntity typeEntity) {
        await Task.Run(async () => {
            _context.Set<TypeEntity>().Remove(typeEntity);
            await _context.SaveChangesAsync();
        });
    }

    public async Task<IEnumerable<TypeEntity>> GetAllAsync()
        => await _context.Set<TypeEntity>().ToListAsync();
    public async Task<IEnumerable<TypeEntity>> GetAsync(Expression<Func<TypeEntity, Boolean>> predicate) 
        => await _context.Set<TypeEntity>().Where(predicate: predicate).ToListAsync();
    public async Task<IEnumerable<TypeEntity>> GetAsync(Expression<Func<TypeEntity, Boolean>>? predicate = null, Func<IQueryable<TypeEntity>, IOrderedQueryable<TypeEntity>>? orderBy = null, String? includeString = null, Boolean disableTracking = true) {
        IQueryable<TypeEntity> query = _context.Set<TypeEntity>();
        if(disableTracking) {
            query = query.AsNoTracking();
        }

        if(String.IsNullOrEmpty(includeString) is false) {
            query.Include(includeString);
        }
        if(predicate is not null) {
            query = query.Where(predicate);
        }

        if(orderBy is not null) {
            query = orderBy(query);
        }
        return await query.ToListAsync();
    }

    public async Task<TypeEntity?> GetByIdAsync(Guid id) {
        return await _context.Set<TypeEntity>().FindAsync(id);
    }

    public async Task UpdateAsync(TypeEntity typeEntity) {
        await Task.Run(async () => {
            _context.Set<TypeEntity>().Update(typeEntity);
            await _context.SaveChangesAsync();
        });
    }
}