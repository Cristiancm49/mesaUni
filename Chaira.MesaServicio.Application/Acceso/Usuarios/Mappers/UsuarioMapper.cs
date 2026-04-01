using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Mappers;

public static class UsuarioMapper
{
    public static UsuarioDto ToDto(Usuario entity)
    {
        return new UsuarioDto
        {
            Id = entity.Id,
            NombreCompleto = entity.NombreCompleto,
            Email = entity.Email,
            Telefono = entity.Telefono,
            IdRol = entity.IdRol,
            NombreRol = entity.Rol?.NombreRol,
            FechaCreacion = entity.FechaCreacion
        };
    }

    public static Usuario ToEntity(UsuarioCreateDto createDto)
    {
        return new Usuario
        {
            NombreCompleto = createDto.NombreCompleto,
            Email = createDto.Email,
            Telefono = createDto.Telefono,
            IdRol = createDto.IdRol,
            IdUsuarioCreacion = createDto.IdUsuarioCreacion,
            FechaCreacion = DateTime.Now
        };
    }

    public static void ApplyUpdate(Usuario entity, UsuarioUpdateDto updateDto)
    {
        if (!string.IsNullOrWhiteSpace(updateDto.NombreCompleto))
        {
            entity.NombreCompleto = updateDto.NombreCompleto;
        }

        if (!string.IsNullOrWhiteSpace(updateDto.Email))
        {
            entity.Email = updateDto.Email;
        }

        if (updateDto.Telefono != null)
        {
            entity.Telefono = updateDto.Telefono;
        }

        if (updateDto.IdRol.HasValue)
        {
            entity.IdRol = updateDto.IdRol.Value;
        }
    }
}

