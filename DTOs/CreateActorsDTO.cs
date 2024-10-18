namespace MimimalAPiPeliculas.DTOs;

public class CreateActorsDTO
{
    public string Name { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public IFormFile? Picture { get; set; } = null!;
}