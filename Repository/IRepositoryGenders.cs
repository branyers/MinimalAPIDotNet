using MimimalAPiPeliculas.Entities;

namespace MimimalAPiPeliculas.Repository;

public interface IRepositoryGenders
{
    Task<int> Create(Gender gender);
    Task<List<Gender>> GetAll();
    Task<Gender?> GetById(int id); 
    Task<bool> Exist (int id);
    Task Update(Gender gender);
    Task Delete(int id);



}