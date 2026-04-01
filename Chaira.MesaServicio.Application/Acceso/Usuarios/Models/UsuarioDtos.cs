using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Models;

public class UsuarioDto
{
    public long Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public long IdRol { get; set; }
    public string? NombreRol { get; set; }
    public DateTime FechaCreacion { get; set; }
}

public class UsuarioCreateDto
{
    [Required(ErrorMessage = "El nombre completo es requerido")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
    [EmailAddress(ErrorMessage = "El formato del email no es valido")]
    public string Email { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "El telefono no puede exceder 20 caracteres")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El ID del rol es requerido")]
    public long IdRol { get; set; }

    [Required(ErrorMessage = "El ID del usuario de creacion es requerido")]
    public long IdUsuarioCreacion { get; set; }
}

public class UsuarioUpdateDto
{
    [Required(ErrorMessage = "El ID del usuario es requerido")]
    public long Id { get; set; }

    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    public string? NombreCompleto { get; set; }

    [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
    [EmailAddress(ErrorMessage = "El formato del email no es valido")]
    public string? Email { get; set; }

    [StringLength(20, ErrorMessage = "El telefono no puede exceder 20 caracteres")]
    public string? Telefono { get; set; }

    public long? IdRol { get; set; }
}

