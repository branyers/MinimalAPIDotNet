using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MimimalAPiPeliculas.DTOs;
using MimimalAPiPeliculas.Entities;
using MimimalAPiPeliculas.Repository;

namespace MimimalAPiPeliculas.Endpoints;

public static class GenderEndpoints
{

    public static RouteGroupBuilder MapGenders(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("gender-get"));
        endpoints.MapGet("/{id:int}", GetById);
        endpoints.MapPost("/", Create);
        endpoints.MapPut("/{id:int}", Update);
        endpoints.MapDelete("/{id:int}", Delete);
        
        return endpoints;
    }
    
    
    static async Task<Results<Ok<GenderDto>, NotFound>> GetById (IRepositoryGenders repository, int id, IMapper mapper)
    {
        var gender = await repository.GetById(id);
        
        if (gender is null)
        {
            return TypedResults.NotFound();
        }
        
        var genderDto = mapper.Map<GenderDto>(gender);
        return TypedResults.Ok(genderDto);
    }


    static async Task<Created<GenderDto>> Create (IMapper mapper, CreateGenderDTO createGenderDto, IRepositoryGenders repository, IOutputCacheStore outputCacheStore)
    {
        var gender = mapper.Map<Gender>(createGenderDto);
        var id = await repository.Create(gender);
        await outputCacheStore.EvictByTagAsync("gender-get",default);
        var genderDto = mapper.Map<GenderDto>(gender);
        return TypedResults.Created($"/genders/{id}", genderDto);
    }

    static async Task<Results<NoContent, NotFound>> Update (int id, CreateGenderDTO createGenderDto, IRepositoryGenders repository, IOutputCacheStore outputCacheStore, IMapper mapper)
    {
        var exist = await repository.Exist(id);

        if (!exist)
        {
            return TypedResults.NotFound();
        }
        
        var genderDto = mapper.Map<Gender>(createGenderDto);
        genderDto.Id = id;
        await repository.Update(genderDto);
        await outputCacheStore.EvictByTagAsync("gender-get",default);
        return TypedResults.NoContent();
    }


    static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositoryGenders repository, IOutputCacheStore outputCacheStore)
    {
        var exist = await repository.Exist(id);

        if (!exist)
        {
            return TypedResults.NotFound();
        }

        await repository.Delete(id);
        await outputCacheStore.EvictByTagAsync("gender-get", default);
        return TypedResults.NoContent();
    }

    static async Task<Ok<List<GenderDto>>> GetAll(IRepositoryGenders repository, IMapper mapper)
    {
        var genders = await repository.GetAll();
        var genderDto = mapper.Map<List<GenderDto>>(genders);
        return TypedResults.Ok(genderDto);
    }

}