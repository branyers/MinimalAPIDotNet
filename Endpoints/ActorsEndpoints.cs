using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MimimalAPiPeliculas.DTOs;
using MimimalAPiPeliculas.Entities;
using MimimalAPiPeliculas.Repository;
using MimimalAPiPeliculas.Services;

namespace MimimalAPiPeliculas.Endpoints;

public static class ActorsEndpoints
{
    private const string container = "actors";

    public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
    {
        group.MapPost("/", Create).DisableAntiforgery();
        group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actors-get"));;
        group.MapGet("/{id:int}", GetById);
        group.MapGet("getActorsByName/{name}", GetActorsByName);
        group.MapDelete("images/{id:int}", DeleteImages);
        return group;
    }
    
    
    
    
    static async Task<Created<ActorDto>> Create(
        [FromForm] CreateActorsDTO createActorDTO,
        IRepositoryActors repository, 
        IOutputCacheStore outputCacheStore, 
        IMapper mapper, 
        IStorageFile storageFile)
    {
        var actor = mapper.Map<Actor>(createActorDTO);

        if (createActorDTO.Picture != null)
        {
            var imageStream = createActorDTO.Picture.OpenReadStream();
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(createActorDTO.Picture.FileName)}_{Guid.NewGuid()}{Path.GetExtension(createActorDTO.Picture.FileName)}";
            var url = await storageFile.StorageFile(imageStream, uniqueFileName, container);    
            actor.Picture = url;
        }

        var id = await repository.Create(actor);
        await outputCacheStore.EvictByTagAsync("actors-get", default);
        var actorDTO = mapper.Map<ActorDto>(actor);
        return TypedResults.Created($"/actors/{id}", actorDTO);
    }
    
    
    static async Task<Ok<List<ActorDto>>> GetAll(IRepositoryActors repository, IMapper mapper)
    {
        var actors = await repository.GetAll();
        var actorsDto = mapper.Map<List<ActorDto>>(actors);
        return TypedResults.Ok(actorsDto);
    }

    
    static async Task<Results<Ok<ActorDto>, NoContent>> GetById(int id, IRepositoryActors repository, IMapper mapper)
    {
        var actor = await repository.GetById(id);

        if (actor is null)
        {
            return TypedResults.NoContent();
        }

        var actorDto = mapper.Map<ActorDto>(actor);
        return TypedResults.Ok(actorDto);
    }
    
    
    static async Task<Results<NoContent, NotFound>> DeleteImages (int id, IStorageFile storageFile, IRepositoryActors repository, IOutputCacheStore outputCacheStore)
    {
        var actor = await repository.GetById(id);
        
        if (actor is null)
        {
            return TypedResults.NotFound();
        }
        
        await storageFile.DeleteFile(actor.Picture, container);
        await outputCacheStore.EvictByTagAsync("actors-get", default);
        return TypedResults.NoContent();
    }
    
    
    
    static async Task<Ok<List<ActorDto>>> GetActorsByName(string name ,IRepositoryActors repository, IMapper mapper)
    {
        var actors = await repository.GetByActorName(name);
        var actorsDto = mapper.Map<List<ActorDto>>(actors);
        return TypedResults.Ok(actorsDto);
    }

    
    
    
}