using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;

namespace WebApi.Data.Repositories;

public class ScheduleSlotRepository(ApplicationDbContext context) : Repository<ScheduleSlotEntity>(context), IScheduleSlotRepository
{
    private readonly ApplicationDbContext _context = context;
}
