using Microsoft.EntityFrameworkCore;
using MimimalAPiPeliculas.Entities;

namespace MimimalAPiPeliculas.Repository;

public class RepositoryGender : IRepositoryGenders
{
    private readonly ApplicationDbContext context;
    
    public RepositoryGender(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    public async Task<int> Create(Gender gender)
    {
        context.Add(gender);
        await context.SaveChangesAsync();
        return gender.Id;
    }

    public async Task<List<Gender>> GetAll()
    {
        return await context.Genders.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Gender?> GetById(int id)
    {
        return await context.Genders.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> Exist(int id)
    {
        return await context.Genders.AnyAsync(x => x.Id == id);
    }

    public async Task Update(Gender gender)
    {
        context.Genders.Update(gender);
        await context.SaveChangesAsync();
        
        
    }

    public async Task Delete(int id)
    {
         await context.Genders.Where(x => x.Id == id).ExecuteDeleteAsync();
         await context.SaveChangesAsync();
    }
}