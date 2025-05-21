using System.Diagnostics;
using WebApi.Data.Entities;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;
    public async Task<ServiceResult<bool>> CreateAsync(EventForm form)
    {
        try
        {
            var alreadyExist = await _eventRepository.GetAsync(findBy: x => x.Name == form.Name);
            if(alreadyExist != null) ServiceResult<bool>.Conflict("An event with that name already exists");

            bool result = await _eventRepository.CreateAsync(form.MapTo<EventEntity>());
            if (result == false) ServiceResult<bool>.Error("Failed to create event");

            return ServiceResult<bool>.Ok();

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to create event");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _eventRepository.GetAsync(findBy: x => x.Id == id);
            if (entity == null) ServiceResult<bool>.BadRequest("Event not found");

            bool result = await _eventRepository.DeleteAsync(entity);
            if (result == false) ServiceResult<bool>.Error("Failed to delete event");

            return ServiceResult<bool>.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to delete event");
        }
    }

    public async Task<ServiceResult<IEnumerable<Event>>> GetAllAsync()
    {
        try
        {
            var entities = await _eventRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Event>>.Ok(entities.Select(x => x.MapTo<Event>()));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<IEnumerable<Event>>.Error("Failed to fetch events");
        }
    }

    public async Task<ServiceResult<Event>> GetAsync(int id)
    {
        try
        {
            var entity = await _eventRepository.GetAsync(findBy: x => x.Id == id);
            if (entity is null) return ServiceResult<Event>.BadRequest("Event not found");

            return ServiceResult<Event>.Ok(entity.MapTo<Event>());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Event>.Error("Failed to fecth event");
        }
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, EventForm form)
    {
        try
        {
            var entity = await _eventRepository.GetAsync(findBy: x => x.Id == id);
            if (entity is null) return ServiceResult<bool>.Conflict("Event not found");

            entity.Name = form.Name;
            entity.Description = form.Description;
            entity.Location = form.Location;
            entity.Price = form.Price;
            entity.StartTime = form.StartTime;
            entity.EndTime = form.EndTime;
            entity.TotalTickets = form.TotalTickets;
            entity.CategoryId = form.CategoryId;
            entity.ScheduleId = form.ScheduleId;

            bool result = await _eventRepository.UpdateAsync(entity);
            if (result == false) ServiceResult<bool>.Error("Failed to update event");

            return ServiceResult<bool>.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to update event");
        }
    }
}
