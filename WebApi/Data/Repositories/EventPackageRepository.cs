using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;

namespace WebApi.Data.Repositories;

public class EventPackageRepository(ApplicationDbContext context) : Repository<EventPackageEntity>(context), IEventPackageRepository
{
    private readonly ApplicationDbContext _context = context;
}
