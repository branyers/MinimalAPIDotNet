using Firebase.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using MimimalAPiPeliculas;
using MimimalAPiPeliculas.Endpoints;
using MimimalAPiPeliculas.Repository;
using MimimalAPiPeliculas.Services;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetValue<string>("AllowOrigins")!;
builder.Services.Configure<FirebaseUserConfig>(builder.Configuration.GetSection("FirebaseUserConfig"));

//Begin Services area

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();

    });
    
    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryGenders, RepositoryGender>();
builder.Services.AddScoped<IRepositoryActors, RepositoryActor>();
builder.Services.Configure<FirebaseUserConfig>(builder.Configuration.GetSection("FirebaseUserConfig"));


builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();

builder.Services.AddScoped<IStorageFile, StorageLocalFile>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));





    

//Finish Services area


var app = builder.Build();

//Begin Middleware area

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseCors();


app.MapGet("/", [EnableCors(policyName:"free")] () => "Hello World!");
app.MapGroup("/genders").MapGenders();
app.MapGroup("/actors").MapActors();

//Finish Middleware area
app.Run(); 

