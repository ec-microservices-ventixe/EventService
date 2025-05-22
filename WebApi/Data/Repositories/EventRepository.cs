using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;
using WebApi.Extensions;

namespace WebApi.Data.Repositories;

public class EventRepository(ApplicationDbContext context) : Repository<EventEntity>(context), IEventRepository
{
    private readonly ApplicationDbContext _context = context;

    public override async Task<IEnumerable<EventEntity>> GetAllAsync(bool orderByDecending = false, Expression<Func<EventEntity, object>>? sortBy = null, Expression<Func<EventEntity, bool>>? filterBy = null, params Expression<Func<EventEntity, object>>[] joins)
    {
        IQueryable<EventEntity> query = _context.Set<EventEntity>()
            .Include(x => x.Category)
            .Include(x => x.Schedule)
            .ThenInclude(x => x.ScheduleSlots);

        if (filterBy != null)
            query = query.Where(filterBy);

        if (joins != null && joins.Length != 0)
            foreach (var join in joins)
                query = query.Include(join);

        if (sortBy != null)
            query = orderByDecending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        return await query.ToListAsync();
    }

    public override async Task<EventEntity> GetAsync(Expression<Func<EventEntity, bool>> findBy, params Expression<Func<EventEntity, object>>[] joins)
    {
        if (findBy == null) return null!;

        IQueryable<EventEntity> query = _context.Set<EventEntity>()
            .Include(x => x.Category)
            .Include(x => x.Schedule)
            .ThenInclude(x => x.ScheduleSlots);

        if (joins != null && joins.Length != 0)
            foreach (var join in joins)
                query = query.Include(join);

        var eventEntity = await query.FirstOrDefaultAsync(findBy) ?? null!;
        return eventEntity;
    }
}
