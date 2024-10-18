using Microsoft.AspNetCore.Components.Sections;
using Microsoft.EntityFrameworkCore;
using MimimalAPiPeliculas.Entities;

namespace MimimalAPiPeliculas.Repository;

public class RepositoryActor: IRepositoryActors
{
    private readonly ApplicationDbContext _context;
    
    public RepositoryActor(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Actor actor)
    {
        _context.Add(actor);
        await _context.SaveChangesAsync();
        return actor.Id;
    }

    public async Task<List<Actor>> GetAll()
    {
        return await _context.Actors.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Actor?> GetById(int id)
    {
        return await _context.Actors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Actors.AnyAsync(x => x.Id == id);
    }

    public async Task Update(Actor actor)
    {
        _context.Actors.Update(actor);
        await _context.SaveChangesAsync();
        
        
    }

    public async Task Delete(int id)
    {
        await _context.Actors.Where(x => x.Id == id).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<List<Actor>> GetByActorName(string name)
    {
        return await _context.Actors.Where(a  => a.Name.Contains(name)).ToListAsync();
    }
}