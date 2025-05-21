using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Interfaces;

namespace WebApi.Data.Repositories;

public class EventScheduleRepository(ApplicationDbContext context) : Repository<EventScheduleEntity>(context), IEventScheduleRepository
{
    private readonly ApplicationDbContext _context = context;
}
