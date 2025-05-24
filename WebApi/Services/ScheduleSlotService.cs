using System.Diagnostics;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;
using WebApi.Data.Repositories;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Models;
namespace WebApi.Services;

public class ScheduleSlotService(IScheduleSlotRepository scheduleSlotRepository, IEventRepository eventRepository) : IScheduleSlotService
{
    private readonly IScheduleSlotRepository _scheduleSlotRepository = scheduleSlotRepository;
    private readonly IEventRepository _eventRepository = eventRepository;

    public async Task<ServiceResult<ScheduleSlot>> CreateAsync(ScheduleSlotForm form)
    {
        try
        {
            var scheduleEntity = await _eventRepository.GetAsync(findBy: x => x.Id == form.EventId);
            if (scheduleEntity is null) return ServiceResult<ScheduleSlot>.BadRequest("Event not found");

            var createdEntity = await _scheduleSlotRepository.CreateAsync(form.MapTo<ScheduleSlotEntity>());
            if (createdEntity is null) return ServiceResult<ScheduleSlot>.Error("Failed to create slot");

            return ServiceResult<ScheduleSlot>.Ok(createdEntity.MapTo<ScheduleSlot>());

        } catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<ScheduleSlot>.Error("Failed to create slot");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var exisitingEntity = await _scheduleSlotRepository.GetAsync(findBy: x => x.Id == id);
            if (exisitingEntity is null) return ServiceResult<bool>.BadRequest("Schedule slot not found");

            bool result = await _scheduleSlotRepository.DeleteAsync(exisitingEntity);
            if (!result) return ServiceResult<bool>.Error("Failed to delete slot");

            return ServiceResult<bool>.NoContent();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to delete slot");
        }
    }

    public async Task<ServiceResult<IEnumerable<ScheduleSlot>>> GetAllAsync()
    {
        try
        {
            var entities = await _scheduleSlotRepository.GetAllAsync();
            var mapped = entities.Select(x => x.MapTo<ScheduleSlot>()).ToList();

            return ServiceResult<IEnumerable<ScheduleSlot>>.Ok(mapped);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<IEnumerable<ScheduleSlot>>.Error("Failed to fetch slots");
        }
    }

    public async  Task<ServiceResult<ScheduleSlot>> GetAsync(int id)
    {
        try
        {
            var exisitingEntity = await _scheduleSlotRepository.GetAsync(findBy: x => x.Id == id); ;
            if (exisitingEntity is null) return ServiceResult<ScheduleSlot>.BadRequest("Schedule slot not found");

            return ServiceResult<ScheduleSlot>.Ok(exisitingEntity.MapTo<ScheduleSlot>());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<ScheduleSlot>.Error("Failed to fetch slot");
        }
    }

    public async Task<ServiceResult<ScheduleSlot>> UpdateAsync(int id, ScheduleSlotForm form)
    {
        try
        {
            var exisitingEntity = await _scheduleSlotRepository.GetAsync(findBy: x => x.Id == id);
            if (exisitingEntity is null) return ServiceResult<ScheduleSlot>.BadRequest("Schedule slot not found");

            exisitingEntity.Name = form.Name;
            exisitingEntity.StartTime = form.StartTime;
            exisitingEntity.StartTime = form.StartTime;
            exisitingEntity.EndTime = form.EndTime;
            
            var updatedEntity = await _scheduleSlotRepository.UpdateAsync(exisitingEntity); ;
            if (updatedEntity is null) return ServiceResult<ScheduleSlot>.Error("Failed to update slot");

            return ServiceResult<ScheduleSlot>.Ok(updatedEntity.MapTo<ScheduleSlot>());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<ScheduleSlot>.Error("Failed to update slot");
        }
    }
}
