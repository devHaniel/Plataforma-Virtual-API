using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;


public class AuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRolRepository _rolRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IRolRepository rolRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var usuarioExistente = await _usuarioRepository.ObtenerPorEmailAsync(dto.Email);

        if (usuarioExistente is not null)
            throw new Exception("El email ya está registrado.");

        var rol = await _rolRepository.ObtenerPorIdAsync(dto.RolId);

        if (rol is null)
            throw new Exception("El rol no existe.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            RolId = dto.RolId,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        await _usuarioRepository.CrearAsync(usuario);

        usuario.Rol = rol;

        var token = _jwtService.GenerarToken(usuario);

        return new AuthResponseDto
        {
            UsuarioId = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = rol.Nombre,
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(dto.Email);

        if (usuario is null)
            throw new Exception("Credenciales incorrectas.");

        if (!usuario.Activo)
            throw new Exception("El usuario está desactivado.");

        var passwordValida = _passwordHasher.VerifyPassword(dto.Password, usuario.PasswordHash);

        if (!passwordValida)
            throw new Exception("Credenciales incorrectas.");

        var token = _jwtService.GenerarToken(usuario);

        return new AuthResponseDto
        {
            UsuarioId = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.Nombre,
            Token = token
        };
    }
}