using System.Diagnostics;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;
using WebApi.Data.Repositories;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class ScheduleService(IEventScheduleRepository scheduleRepository) : IScheduleService
{
    private readonly IEventScheduleRepository _scheduleRepository = scheduleRepository;

    public async Task<ServiceResult<Schedule>> CreateAsync(ScheduleForm form)
    {
        try
        {
            var existingEntity = await _scheduleRepository.GetAsync(findBy: x => x.EventId == form.EventId);
            if (existingEntity is not null) return ServiceResult<Schedule>.Conflict("A scehdule with that event id already exists");

            var createdEntity = await _scheduleRepository.CreateAsync(form.MapTo<EventScheduleEntity>());
            if (createdEntity is null) return ServiceResult<Schedule>.Error("Failed to create Schedule");
            return ServiceResult<Schedule>.Ok(createdEntity.MapTo<Schedule>());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Schedule>.Error("Failed to create Schedule");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var existingEntity = await _scheduleRepository.GetAsync(findBy: x => x.Id == id);
            if (existingEntity is null) return ServiceResult<bool>.BadRequest("Schedule not found");
            bool result = await _scheduleRepository.DeleteAsync(existingEntity);
            if (!result) return ServiceResult<bool>.Error("Failed to delete schedule");
            return ServiceResult<bool>.NoContent();


        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to delete schedule");
        }
    }

    public async Task<ServiceResult<Schedule>> GetAsync(int id)
    {
        try
        {
            var entity = await _scheduleRepository.GetAsync(findBy: x => x.Id == id, joins: x => x.ScheduleSlots);
            if (entity is null) return ServiceResult<Schedule>.BadRequest("Schedule not found");

            var schedule = entity.MapTo<Schedule>();
            schedule.ScheduleSlots = entity.ScheduleSlots.Select(x => x.MapTo<ScheduleSlot>());
            return ServiceResult<Schedule>.Ok(schedule);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Schedule>.Error("Failed to fetch Schedule");
        }
    }
}
