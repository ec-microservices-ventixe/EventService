using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Interfaces;

namespace WebApi.Data.Repositories;

public class EventRepository(ApplicationDbContext context) : Repository<EventEntity>(context), IEventRepository
{
    private readonly ApplicationDbContext _context = context;
}
