using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Interfaces;

namespace WebApi.Data.Repositories;

public class EventCategoryRepository(ApplicationDbContext context) : Repository<EventCategoryEntity>(context), IEventCategoryRepository
{
    private readonly ApplicationDbContext _context = context;
}
