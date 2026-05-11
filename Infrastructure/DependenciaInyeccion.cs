
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DependenciaInyeccion
{
    public static IServiceCollection AgregarInfra(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<AuthService>();

        // Repositorios
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<ICursoRepository, CursoRepository>();
        services.AddScoped<IModuloRepository, ModuloRepository>();
        services.AddScoped<ILeccionRepository, LeccionRepository>();
        services.AddScoped<IInscripcionRepository, InscripcionRepository>();
        services.AddScoped<ITareaRepository, TareaRepository>();
        services.AddScoped<IEntregaRepository, EntregaRepository>();

        // Servicios
        services.AddScoped<CursoService>();
        services.AddScoped<LeccionService>();
        services.AddScoped<TareaService>();
        services.AddScoped<EntregaService>();
        services.AddScoped<IFileService, FileService>();

        return services;

    }
}
