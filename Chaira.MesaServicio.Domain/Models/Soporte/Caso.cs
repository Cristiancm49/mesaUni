using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Acceso;
using Chaira.MesaServicio.Domain.Models.Catalogo;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Models.Soporte
{
    [Table("Caso", Schema = "soporte")]
    public class Caso
    {
        [Key]
        [Column("IdCaso")]
        public long Id { get; set; }

        [Column("Descripcion")]
        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Column("IdUsuarioReporta")]
        [Required]
        public long IdUsuarioReporta { get; set; }

        [Column("TelefonoContacto")]
        [StringLength(20)]
        public string? TelefonoContacto { get; set; }

        [Column("CorreoContacto")]
        [StringLength(150)]
        public string? CorreoContacto { get; set; }

        [Column("IdEstadoCaso")]
        [Required]
        public long IdEstadoCaso { get; set; }

        [Column("FechaRegistro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("FechaAceptacion")]
        public DateTime? FechaAceptacion { get; set; }

        [Column("NumeroCaso")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? NumeroCaso { get; set; }

        [Column("FechaResolucion")]
        public DateTime? FechaResolucion { get; set; }

        [Column("FechaCierre")]
        public DateTime? FechaCierre { get; set; }

        [Column("IdTipoCaso")]
        [Required]
        public long IdTipoCaso { get; set; }

        [Column("IdActivo")]
        public long? IdActivo { get; set; }

        [Column("IdAreaTecnica")]
        public long? IdAreaTecnica { get; set; }

        [Column("IdPrioridad")]
        [Required]
        public long IdPrioridad { get; set; }

        [Column("IdCanalIngreso")]
        [Required]
        public long IdCanalIngreso { get; set; }

        [Column("IdTecnicoAsignado")]
        public long? IdTecnicoAsignado { get; set; }

        [Column("FechaActualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdEstadoCaso")]
        public virtual EstadoCaso EstadoCaso { get; set; } = null!;

        [ForeignKey("IdTipoCaso")]
        public virtual TipoCaso TipoCaso { get; set; } = null!;

        [ForeignKey("IdActivo")]
        public virtual Activo? Activo { get; set; }

        [ForeignKey("IdAreaTecnica")]
        public virtual AreaTecnica? AreaTecnica { get; set; }

        [ForeignKey("IdPrioridad")]
        public virtual Prioridad Prioridad { get; set; } = null!;

        [ForeignKey("IdCanalIngreso")]
        public virtual CanalIngreso CanalIngreso { get; set; } = null!;

        [ForeignKey("IdUsuarioReporta")]
        public virtual Usuario UsuarioReporta { get; set; } = null!;

        [ForeignKey("IdTecnicoAsignado")]
        public virtual Usuario? TecnicoAsignado { get; set; }

        public virtual ICollection<TrazabilidadCaso> Trazabilidades { get; set; } = new List<TrazabilidadCaso>();
        public virtual ICollection<EncuestaCalidad> Encuestas { get; set; } = new List<EncuestaCalidad>();
    }
}















