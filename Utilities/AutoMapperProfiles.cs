using AutoMapper;
using MimimalAPiPeliculas.DTOs;
using MimimalAPiPeliculas.Entities;

namespace MimimalAPiPeliculas;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CreateGenderDTO, Gender>();
        CreateMap<Gender, GenderDto>();

        CreateMap<CreateActorsDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());
        CreateMap<Actor, ActorDto>();
    }
}