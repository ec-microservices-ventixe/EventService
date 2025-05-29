using Grpc.Core;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class EventService(IEventRepository eventRepository, IFileService fileService, IEventCategoryRepository categoryRepository, EventInfoGrpcService eventInfoGrpcService) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IFileService _fileService = fileService;
    private readonly IEventCategoryRepository _categoryRepository = categoryRepository;
    private readonly EventInfoGrpcService _eventInfoGrpcService = eventInfoGrpcService; 
    public async Task<ServiceResult<Event>> CreateAsync(EventForm form)
    {
        try
        {
            var alreadyExist = await _eventRepository.GetAsync(findBy: x => x.Name == form.Name);
            if(alreadyExist != null) return ServiceResult<Event>.Conflict("An event with that name already exists");

            if(form.ImageFile is not null)
                form.ImageUrl = await _fileService.UploadFileAsync(form.ImageFile);

            var mappedForm = form.MapTo<EventEntity>();
            if (form.CategoryId != 0)
            {
                var category = await _categoryRepository.GetAsync(findBy: x => x.Id == form.CategoryId);
                if (category is null) return ServiceResult<Event>.BadRequest("category not found");
                mappedForm.Category = category;
            }

            await _eventRepository.BeginTransactionAsync();

            var createdEntity = await _eventRepository.CreateAsync(mappedForm);
            if (createdEntity is null) return ServiceResult<Event>.Error("Failed to create event");

            var grpcResult = await _eventInfoGrpcService.UpdateEventInfoAsync(createdEntity.Id, createdEntity.TotalTickets);
            if (grpcResult.Success == false)
            {
                await _eventRepository.RollbackTransactionAsync();
                return ServiceResult<Event>.Error("Failed to create event");
            }
            await _eventRepository.CommitTransactionAsync();
            return ServiceResult<Event>.Ok(createdEntity.MapTo<Event>());

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await _eventRepository.RollbackTransactionAsync();

            return ServiceResult<Event>.Error("Failed to create event");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _eventRepository.GetAsync(findBy: x => x.Id == id);
            if (entity is null) return ServiceResult<bool>.BadRequest("Event not found");

            await _eventRepository.BeginTransactionAsync();
            bool result = await _eventRepository.DeleteAsync(entity);
            if (result == false) ServiceResult<bool>.Error("Failed to delete event");

            var grpcResult = await _eventInfoGrpcService.DeleteEventInfoAsync(id);
            if (grpcResult.Success == false)
            {
                await _eventRepository.RollbackTransactionAsync();
                return ServiceResult<bool>.Error("Failed to delete event");
            }
            await _eventRepository.CommitTransactionAsync();
            return ServiceResult<bool>.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await _eventRepository.RollbackTransactionAsync();
            return ServiceResult<bool>.Error("Failed to delete event");
        }
    }

    public async Task<ServiceResult<IEnumerable<Event>>> GetAllAsync()
    {
        try
        {
            var entities = await _eventRepository.GetAllAsync(joins: [c => c.Category, s => s.ScheduleSlots, p => p.EventPackages]);
            return ServiceResult<IEnumerable<Event>>.Ok(entities.Select(x =>
            {
                var mappedEvents = x.MapTo<Event>();
                mappedEvents.ScheduleSlots = x.ScheduleSlots.Select(x => x.MapTo<ScheduleSlot>()).ToList();
                mappedEvents.Packages = x.EventPackages.Select(x => x.MapTo<Package>()).ToList();
                return mappedEvents;
            }));
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
            var entity = await _eventRepository.GetAsync(findBy: x => x.Id == id, joins: [c => c.Category, s => s.ScheduleSlots, p => p.EventPackages]);
            if (entity is null) return ServiceResult<Event>.BadRequest("Event not found");

            var mappedEvent = entity.MapTo<Event>();
            mappedEvent.ScheduleSlots = entity.ScheduleSlots.Select(x => x.MapTo<ScheduleSlot>()).ToList();
            mappedEvent.Packages = entity.EventPackages.Select(x => x.MapTo<Package>()).ToList();

            return ServiceResult<Event>.Ok(mappedEvent);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Event>.Error("Failed to fecth event");
        }
    }

    public async Task<ServiceResult<Event>> UpdateAsync(int id, EventForm form)
    {
        try
        {
            var entity = await _eventRepository.GetAsync(findBy: x => x.Id == id);
            if (entity is null) return ServiceResult<Event>.BadRequest("Event not found");
            entity.Name = form.Name;
            entity.Description = form.Description;
            entity.StartTime = form.StartTime;
            entity.EndTime = form.EndTime;  
            entity.Date = form.Date;
            entity.Location = form.Location;
            if(form.ImageFile != null)
            {
                entity.ImageUrl = await _fileService.UploadFileAsync(form.ImageFile);
            }
            if(form.CategoryId != 0)
            {
                var category = await _categoryRepository.GetAsync(findBy: x => x.Id == form.CategoryId);
                if(category is null) return ServiceResult<Event>.BadRequest("category not found");
                entity.Category = category; 
            }
            await _eventRepository.BeginTransactionAsync();

            var updatedEntity = await _eventRepository.UpdateAsync(entity);
            if (updatedEntity is null) return ServiceResult<Event>.Error("Failed to update event");

            var grpcResult = await _eventInfoGrpcService.UpdateEventInfoAsync(updatedEntity.Id, updatedEntity.TotalTickets);
            if (grpcResult.Success == false)
            {
                await _eventRepository.RollbackTransactionAsync();
                return ServiceResult<Event>.Error("Failed to update event");
            }
            await _eventRepository.CommitTransactionAsync();
            return ServiceResult<Event>.Ok(updatedEntity.MapTo<Event>());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await _eventRepository.RollbackTransactionAsync();
            return ServiceResult<Event>.Error("Failed to update event");
        }
    }
}
