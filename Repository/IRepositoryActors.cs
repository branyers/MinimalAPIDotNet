using MimimalAPiPeliculas.Entities;

namespace MimimalAPiPeliculas.Repository;

public interface IRepositoryActors
{
    Task<int> Create(Actor actor);
    Task<List<Actor>> GetAll();
    Task<Actor?> GetById(int id); 
    Task<bool> Exist (int id);
    Task Update(Actor actor);
    Task Delete(int id);
    Task<List<Actor>> GetByActorName(string name);

}