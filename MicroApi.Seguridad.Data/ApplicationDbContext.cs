using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Models.Acceso;
using MicroApi.Seguridad.Domain.Models.Catalogo;
using MicroApi.Seguridad.Domain.Models.Inventario;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ==================== SCHEMA ACCESO ====================
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // ==================== SCHEMA CATALOGO ====================
        public DbSet<EstadoGeneral> EstadosGenerales { get; set; }
        public DbSet<AreaTecnica> AreasTecnicas { get; set; }
        public DbSet<TipoTrabajo> TiposTrabajos { get; set; }
        public DbSet<EstadoIntervencionTecnica> EstadosIntervencionTecnica { get; set; }
        public DbSet<EstadoCaso> EstadosCaso { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<TipoCaso> TiposCaso { get; set; }
        public DbSet<CanalIngreso> CanalesIngreso { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<CategoriaActivo> CategoriasActivo { get; set; }
        public DbSet<EstadoActivo> EstadosActivo { get; set; }
        public DbSet<TipoConsumible> TiposConsumible { get; set; }
        public DbSet<EstadoConsumible> EstadosConsumible { get; set; }

        // ==================== SCHEMA INVENTARIO ====================
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Activo> Activos { get; set; }
        public DbSet<Componente> Componentes { get; set; }
        public DbSet<Consumible> Consumibles { get; set; }
        public DbSet<HojaDeVidaActivo> HojasDeVidaActivo { get; set; }

        // ==================== SCHEMA SOPORTE ====================
        public DbSet<Caso> Casos { get; set; }
        public DbSet<TrazabilidadCaso> TrazabilidadesCaso { get; set; }
        public DbSet<IntervencionTecnica> IntervencionesTecnicas { get; set; }
        public DbSet<DetalleCambioComponentes> DetallesCambioComponentes { get; set; }
        public DbSet<DetalleConsumible> DetallesConsumible { get; set; }
        public DbSet<RevisionAdmi> RevisionesAdmi { get; set; }
        public DbSet<EncuestaCalidad> EncuestasCalidad { get; set; }
        public DbSet<DetalleEncuesta> DetallesEncuesta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== CONFIGURACIÓN ACCESO ====================
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasOne(r => r.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(r => r.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.Usuarios)
                    .WithOne(u => u.Rol)
                    .HasForeignKey(u => u.IdRol)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==================== CONFIGURACIÓN CATALOGO ====================
            modelBuilder.Entity<AreaTecnica>(entity =>
            {
                entity.HasOne(a => a.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(a => a.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Encargado)
                    .WithMany()
                    .HasForeignKey(a => a.IdEncargado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EstadoCaso>(entity =>
            {
                entity.HasOne(e => e.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(e => e.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EstadoActivo>(entity =>
            {
                entity.HasOne(e => e.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(e => e.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EstadoConsumible>(entity =>
            {
                entity.HasOne(e => e.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(e => e.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EstadoIntervencionTecnica>(entity =>
            {
                entity.HasOne(e => e.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(e => e.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==================== CONFIGURACIÓN INVENTARIO ====================
            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasOne(i => i.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(i => i.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Responsable)
                    .WithMany()
                    .HasForeignKey(i => i.IdResponsableInventario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(i => i.Activos)
                    .WithOne(a => a.Inventario)
                    .HasForeignKey(a => a.IdInventario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(i => i.Componentes)
                    .WithOne(c => c.Inventario)
                    .HasForeignKey(c => c.IdInventario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(i => i.Consumibles)
                    .WithOne(c => c.Inventario)
                    .HasForeignKey(c => c.IdInventario)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Componente>(entity =>
            {
                entity.HasOne(c => c.Inventario)
                    .WithMany(i => i.Componentes)
                    .HasForeignKey(c => c.IdInventario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.EstadoGeneral)
                    .WithMany()
                    .HasForeignKey(c => c.IdEstadoGeneral)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Consumible>(entity =>
            {
                entity.HasOne(c => c.Inventario)
                    .WithMany(i => i.Consumibles)
                    .HasForeignKey(c => c.IdInventario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.TipoConsumible)
                    .WithMany()
                    .HasForeignKey(c => c.IdTipoConsumible)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.EstadoConsumible)
                    .WithMany()
                    .HasForeignKey(c => c.IdEstadoConsumible)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Activo>(entity =>
            {
                entity.HasOne(a => a.CategoriaActivo)
                    .WithMany()
                    .HasForeignKey(a => a.IdCategoriaActivo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.EstadoActivo)
                    .WithMany()
                    .HasForeignKey(a => a.IdEstadoActivo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Inventario)
                    .WithMany(i => i.Activos)
                    .HasForeignKey(a => a.IdInventario)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Ubicacion)
                    .WithMany()
                    .HasForeignKey(a => a.IdUbicacion)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Responsable)
                    .WithMany()
                    .HasForeignKey(a => a.IdResponsableActivo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.HojasDeVida)
                    .WithOne(h => h.Activo)
                    .HasForeignKey(h => h.IdActivo)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<HojaDeVidaActivo>(entity =>
            {
                entity.HasOne(h => h.Activo)
                    .WithMany(a => a.HojasDeVida)
                    .HasForeignKey(h => h.IdActivo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(h => h.UsuarioCreacion)
                    .WithMany()
                    .HasForeignKey(h => h.IdUsuarioCreacion)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==================== CONFIGURACIÓN SOPORTE ====================
            modelBuilder.Entity<Caso>(entity =>
            {
                entity.HasMany(c => c.Trazabilidades)
                    .WithOne(t => t.Caso)
                    .HasForeignKey(t => t.IdCaso)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Encuestas)
                    .WithOne(e => e.Caso)
                    .HasForeignKey(e => e.IdCaso)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TrazabilidadCaso>(entity =>
            {
                entity.HasMany(t => t.IntervencionesTecnicas)
                    .WithOne(i => i.TrazabilidadCaso)
                    .HasForeignKey(i => i.IdTrazabilidadCaso)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IntervencionTecnica>(entity =>
            {
                entity.HasMany(i => i.CambiosComponentes)
                    .WithOne(c => c.IntervencionTecnica)
                    .HasForeignKey(c => c.IdIntervencionTecnica)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(i => i.DetallesConsumibles)
                    .WithOne(d => d.IntervencionTecnica)
                    .HasForeignKey(d => d.IdIntervencionTecnica)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(i => i.RevisionesAdmi)
                    .WithOne(r => r.IntervencionTecnica)
                    .HasForeignKey(r => r.IdIntervencionTecnica)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EncuestaCalidad>(entity =>
            {
                entity.HasMany(e => e.Detalles)
                    .WithOne(d => d.Encuesta)
                    .HasForeignKey(d => d.IdEncuesta)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // EF Core con SQL Server usa OUTPUT en SaveChanges por defecto.
            // Como esta BD tiene triggers de auditoria, se deshabilita OUTPUT para tablas mapeadas.
            ConfigureTablesForTriggers(modelBuilder);
        }

        private static void ConfigureTablesForTriggers(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var tableName = entityType.GetTableName();

                if (clrType == null || string.IsNullOrWhiteSpace(tableName))
                {
                    continue;
                }

                var schema = entityType.GetSchema();

                modelBuilder.Entity(clrType)
                    .ToTable(tableName!, schema, tableBuilder => tableBuilder.UseSqlOutputClause(false));
            }
        }
    }
}
