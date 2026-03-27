
--   CREACIÓN DE BASE DE DATOS Y SCHEMAS

IF DB_ID('MesaServicios') IS NULL
BEGIN
    CREATE DATABASE MesaServicios;
END;
GO

USE MesaServicios;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'acceso')
    EXEC('CREATE SCHEMA acceso');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'catalogo')
    EXEC('CREATE SCHEMA catalogo');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'inventario')
    EXEC('CREATE SCHEMA inventario');
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'soporte')
    EXEC('CREATE SCHEMA soporte');
GO




--   ESTADO GENERAL

CREATE TABLE catalogo.EstadoGeneral (
    IdEstadoGeneral BIGINT IDENTITY(1,1) NOT NULL,
    NombreEstado VARCHAR(100) NOT NULL,   -- Activo / Inactivo
    Descripcion VARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO




--   MÓDULO ACCESO

CREATE TABLE acceso.Rol (
    IdRol BIGINT IDENTITY(1,1) NOT NULL,
    NombreRol VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE acceso.Usuario (
    IdUsuario BIGINT IDENTITY(1,1) NOT NULL,
    NombreCompleto VARCHAR(150) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Telefono VARCHAR(20) NULL,
    IdRol BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO



--   MÓDULO CATÁLOGO

CREATE TABLE catalogo.AreaTecnica (
    IdAreaTecnica BIGINT IDENTITY(1,1) NOT NULL,
    NombreAreaTecnica VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEncargado BIGINT NOT NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.TipoTrabajo (
    IdTipoTrabajo BIGINT IDENTITY(1,1) NOT NULL,
    NombreTipoTrabajo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.EstadoIntervencionTecnica (
    IdEstadoIntervencion BIGINT IDENTITY(1,1) NOT NULL,
    NombreEstado VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.EstadoCaso (
    IdEstadoCaso BIGINT IDENTITY(1,1) NOT NULL,
    NombreEstadoCaso VARCHAR(100) NOT NULL,
    DescripcionEstadoCaso VARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.Prioridad (
    IdPrioridad BIGINT IDENTITY(1,1) NOT NULL,
    NombrePrioridad VARCHAR(100) NOT NULL,
    TiempoRespuestaDias INT NOT NULL,
    TiempoResolucionDias INT NOT NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.TipoCaso (
    IdTipoCaso BIGINT IDENTITY(1,1) NOT NULL,
    NombreTipoCaso VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.CanalIngreso (
    IdCanalIngreso BIGINT IDENTITY(1,1) NOT NULL,
    NombreCanal VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.Pregunta (
    IdPregunta BIGINT IDENTITY(1,1) NOT NULL,
    TextoPregunta VARCHAR(100) NOT NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.Respuesta (
    IdRespuesta BIGINT IDENTITY(1,1) NOT NULL,
    TextoRespuesta VARCHAR(50) NOT NULL,
    ValorNumerico INT NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO



--   INVENTARIO 

CREATE TABLE inventario.Componente (
    IdComponente BIGINT IDENTITY(1,1) NOT NULL,
    NombreComponente VARCHAR(200) NOT NULL,
    Marca VARCHAR(100) NULL,
    Modelo VARCHAR(100) NULL,
    IdInventario BIGINT NOT NULL, 
    StockActual INT NOT NULL DEFAULT 0,
    StockMinimo INT NOT NULL DEFAULT 1,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO




--   MÓDULO SOPORTE

CREATE TABLE soporte.Caso (
    IdCaso BIGINT IDENTITY(1,1) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    IdUsuarioReporta BIGINT NOT NULL,
    TelefonoContacto VARCHAR(20) NULL,
    CorreoContacto VARCHAR(150) NULL,
    IdEstadoCaso BIGINT NOT NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaAceptacion DATETIME2 NULL,
      NumeroCaso AS (
        'CASO-' +
        CONVERT(VARCHAR(4), DATEPART(YEAR, FechaRegistro)) +
        '-' +
        CONVERT(VARCHAR(20), IdCaso)
    ) PERSISTED,
    FechaResolucion DATETIME2 NULL,
    FechaCierre DATETIME2 NULL,
    IdTipoCaso BIGINT NOT NULL,
    IdActivo BIGINT NULL,
    IdAreaTecnica BIGINT NULL,
    IdPrioridad BIGINT NOT NULL,
    IdCanalIngreso BIGINT NOT NULL,
    IdTecnicoAsignado BIGINT NULL,
    FechaActualizacion DATETIME2 NULL,
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE soporte.TrazabilidadCaso (
    IdTrazabilidadCaso BIGINT IDENTITY(1,1) NOT NULL,
    IdCaso BIGINT NOT NULL,
    FechaEvento DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioAccion BIGINT NOT NULL,
    TipoEvento VARCHAR(100) NOT NULL,
    Comentario VARCHAR(MAX) NULL,
    IdEstadoCaso BIGINT NULL,
    IdAreaTecnica BIGINT NULL,
    IdTecnicoAsignado BIGINT NULL
);
GO


ALTER TABLE soporte.TrazabilidadCaso 
ADD CONSTRAINT CKTrazabilidadTipoEvento 
CHECK (
    TipoEvento IN (
        'CasoCreado',
        'TecnicoAsignado',
        'TecnicoReasignado',
        'EstadoCambiado',
        'CasoEscalado',
        'CasoResuelto',
        'CasoCerrado',
        'CasoRechazado',
        'IntervencionCreada',
        'IntervencionRegistrada',
        'IntervencionActualizada',
        'DiagnosticoRegistrado',
        'DiagnosticoActualizado', 
        'IntervencionAprobada',
        'IntervencionRechazada',
        'IntervencionCompletada',
        'ComponentesAgregados',
        'ConsumiblesAgregados',
        'ActivoAsignado',
        'Escalado',
        'EncuestaCreada'
    )
);
GO




CREATE TABLE soporte.IntervencionTecnica (
    IdIntervencionTecnica BIGINT IDENTITY(1,1) NOT NULL,
    IdTrazabilidadCaso BIGINT NOT NULL,
    IdTipoTrabajo BIGINT NOT NULL,
    IdEstadoIntervencion BIGINT NOT NULL,
    FechaInicio DATETIME2 NOT NULL,
    FechaFin DATETIME2 NULL,
    Diagnostico VARCHAR(MAX) NULL,
    SolucionAplicada VARCHAR(MAX) NULL,
    IdUsuarioAccion BIGINT NOT NULL
);
GO


CREATE TABLE soporte.DetalleCambioComponentes (
    IdCambioComponente BIGINT IDENTITY(1,1) NOT NULL,
    IdIntervencionTecnica BIGINT NOT NULL,
    IdComponente BIGINT NOT NULL,
    Cantidad INT NOT NULL DEFAULT 1,
    TipoCambio VARCHAR(20) NOT NULL,   
    DescripcionCambio VARCHAR(MAX) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

ALTER TABLE soporte.DetalleCambioComponentes
ADD CONSTRAINT CK_DetalleCambio_TipoCambio CHECK (
    TipoCambio IN ('INSTALACION', 'RETIRO', 'REEMPLAZO')
);
GO


CREATE TABLE soporte.RevisionAdmi (
    IdRevisionAdmi BIGINT IDENTITY(1,1) NOT NULL,
    IdIntervencionTecnica BIGINT NOT NULL,
    Aprobado BIT NOT NULL DEFAULT 0,
    ObservacionRevision VARCHAR(MAX) NULL,
    TipoRevision VARCHAR(30) NOT NULL DEFAULT 'DIAGNOSTICO',
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE inventario.Ubicacion (
    IdUbicacion BIGINT IDENTITY(1,1) NOT NULL,
    IdSede BIGINT NOT NULL,    
    Bloque VARCHAR(100) NULL,      
    Piso VARCHAR(50) NULL,         
    Sala VARCHAR(150) NULL,        
    Descripcion VARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE catalogo.Sede (
    IdSede BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombreSede VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.CategoriaActivo (
    IdCategoriaActivo BIGINT IDENTITY(1,1) NOT NULL,
    NombreCategoria VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE catalogo.EstadoActivo (
    IdEstadoActivo BIGINT IDENTITY(1,1) NOT NULL,
    NombreEstado VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE inventario.Inventario (
    IdInventario BIGINT IDENTITY(1,1) NOT NULL,
    NombreInventario VARCHAR(200) NOT NULL,  
    Descripcion VARCHAR(MAX) NULL,           
    IdEstadoGeneral BIGINT NOT NULL, 
    IdResponsableInventario BIGINT NOT NULL,       
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE inventario.Activo (
    IdActivo BIGINT IDENTITY(1,1) NOT NULL,
    CodigoPatrimonial VARCHAR(100) NULL,   
    NombreActivo VARCHAR(200) NOT NULL,
    DescripcionTecnica VARCHAR(MAX) NULL,
    Marca VARCHAR(100) NULL,
    Modelo VARCHAR(100) NULL,
    Serie VARCHAR(100) NULL,
    IdCategoriaActivo BIGINT NOT NULL,
    IdEstadoActivo BIGINT NOT NULL,
    IdInventario BIGINT NOT NULL,          
    IdUbicacion BIGINT NOT NULL,                      
    FechaIngreso DATETIME2 NULL,
    IdResponsableActivo BIGINT NULL, 
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NULL,
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE inventario.HojaDeVidaActivo (
    IdHojaActivo BIGINT IDENTITY(1,1) NOT NULL,
    IdActivo BIGINT NOT NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    DetalleRegistro VARCHAR(MAX) NOT NULL,   
    TipoEvento VARCHAR(100) NULL,            
    IdCaso BIGINT NULL,                      
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

ALTER TABLE inventario.HojaDeVidaActivo
ADD CONSTRAINT CKHojaDeVidaActivoTipoEvento
CHECK (
    TipoEvento IN (
        'CasoCreado',
        'CasoCerrado',
        'Traslado',
        'Mantenimiento',
        'Reemplazo',
        'Baja',
        'Actualizacion'
    )
);




CREATE TABLE catalogo.TipoConsumible (
    IdTipoConsumible BIGINT IDENTITY(1,1) NOT NULL,
    NombreTipo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    IdEstadoGeneral BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE catalogo.EstadoConsumible (
    IdEstadoConsumible BIGINT IDENTITY(1,1) NOT NULL,
    NombreEstado VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


CREATE TABLE inventario.Consumible (
    IdConsumible BIGINT IDENTITY(1,1) NOT NULL,
    NombreConsumible VARCHAR(200) NOT NULL,
    Marca VARCHAR(100) NULL,
    Modelo VARCHAR(100) NULL,
    StockActual INT NOT NULL DEFAULT 0,
    StockMinimo INT NOT NULL DEFAULT 1,
    DescripcionTecnica VARCHAR(MAX) NULL,
    IdInventario BIGINT NOT NULL,
    IdTipoConsumible BIGINT NOT NULL,
    IdEstadoConsumible BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NULL,
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE soporte.EncuestaCalidad (
    IdEncuesta BIGINT IDENTITY(1,1) NOT NULL,
    IdCaso BIGINT NOT NULL,
    FechaEncuesta DATETIME2 NOT NULL DEFAULT GETDATE(),
    Observaciones VARCHAR(MAX) NULL,
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE soporte.DetalleEncuesta (
    IdDetalleEncuesta BIGINT IDENTITY(1,1) NOT NULL,
    IdEncuesta BIGINT NOT NULL,
    IdPregunta BIGINT NOT NULL,
    IdRespuesta BIGINT NOT NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO

CREATE TABLE soporte.DetalleConsumible (
    IdDetalleConsumible BIGINT IDENTITY(1,1) NOT NULL,
    IdIntervencionTecnica BIGINT NOT NULL,
    IdConsumible BIGINT NOT NULL,
    Cantidad INT NOT NULL,
    DescripcionUso VARCHAR(MAX) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    IdUsuarioCreacion BIGINT NOT NULL
);
GO


-- CATALOGO INICIAL DE DATOS DE CATALOGO

SET IDENTITY_INSERT acceso.Rol ON;

INSERT INTO acceso.Rol (IdRol, NombreRol, Descripcion, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES (1, 'Administrador', 'Rol administrador del sistema', 1, GETDATE(), 1);

SET IDENTITY_INSERT acceso.Rol OFF;
GO


SET IDENTITY_INSERT acceso.Usuario ON;

INSERT INTO acceso.Usuario (IdUsuario, NombreCompleto, Email, Telefono, IdRol, FechaCreacion, IdUsuarioCreacion)
VALUES (1, 'Administrador del Sistema', 'admin@sistema.com', '0000000000', 1, GETDATE(), 1);

SET IDENTITY_INSERT acceso.Usuario OFF;
GO

INSERT INTO catalogo.EstadoGeneral (NombreEstado, Descripcion, FechaCreacion, IdUsuarioCreacion)
VALUES ('Activo', 'Estado habilitado', GETDATE(), 1),
       ('Inactivo', 'Estado deshabilitado', GETDATE(), 1);
GO


INSERT INTO catalogo.EstadoCaso (NombreEstadoCaso, DescripcionEstadoCaso, FechaCreacion, IdUsuarioCreacion)
VALUES 
('Abierto', 'El caso fue creado',  GETDATE(), 1),
('Asignado', 'El caso fue asignado a un técnico',  GETDATE(), 1),
('En Progreso', 'El técnico está trabajando en el caso',  GETDATE(), 1),
('Resuelto', 'Se aplicó solución técnica',  GETDATE(), 1),
('Cerrado', 'El usuario aprobó la solución',  GETDATE(), 1);
GO


INSERT INTO catalogo.EstadoIntervencionTecnica (NombreEstado, Descripcion, FechaCreacion, IdUsuarioCreacion)
VALUES
('Pendiente Aprobacion', 'Diagnostico registrado y pendiente de revision administrativa',  GETDATE(), 1),
('Aprobada', 'Diagnostico aprobado y listo para ejecutar',  GETDATE(), 1),
('Rechazada', 'Diagnostico observado para correccion tecnica',  GETDATE(), 1),
('Pendiente Aprobacion Solucion', 'Solucion registrada y pendiente de revision administrativa final',  GETDATE(), 1),
('Solucion Rechazada', 'Solucion observada para correccion tecnica',  GETDATE(), 1),
('Completado', 'Trabajo finalizado',  GETDATE(), 1);
GO


INSERT INTO catalogo.Prioridad (NombrePrioridad, TiempoRespuestaDias, TiempoResolucionDias, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES
('Alta', 1, 2, 1, GETDATE(), 1),
('Media', 3, 7, 1, GETDATE(), 1),
('Baja', 7, 14, 1, GETDATE(), 1);
GO


INSERT INTO catalogo.TipoCaso (NombreTipoCaso, Descripcion, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES
('Incidencia', 'Falla o problema técnico', 1, GETDATE(), 1),
('Requerimiento', 'Solicitud de servicio', 1, GETDATE(), 1);
GO


INSERT INTO catalogo.CanalIngreso (NombreCanal, Descripcion, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES
('Web', 'Portal de autoservicio', 1, GETDATE(), 1),
('Telefono', 'Llamadas entrantes al soporte', 1, GETDATE(), 1),
('Correo', 'Correo institucional', 1, GETDATE(), 1);
GO


INSERT INTO catalogo.EstadoActivo (NombreEstado, Descripcion, FechaCreacion, IdUsuarioCreacion)
VALUES
('Operativo', 'El activo funciona correctamente', GETDATE(), 1),
('En Mantenimiento', 'El activo está siendo revisado', GETDATE(), 1),
('Fuera de Servicio', 'El activo no está operativo', GETDATE(), 1);
GO


INSERT INTO catalogo.CategoriaActivo (NombreCategoria, Descripcion, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES
('Computador', 'Equipos de escritorio y portátiles', 1, GETDATE(), 1),
('Impresora', 'Equipos de impresión', 1, GETDATE(), 1),
('Redes', 'Equipos de infraestructura de red', 1, GETDATE(), 1);
GO


INSERT INTO catalogo.TipoConsumible (NombreTipo, Descripcion, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES
('Toner', 'Cartuchos de impresión', 1, GETDATE(), 1),
('Cable', 'Cables de red, energía, etc.', 1, GETDATE(), 1),
('Limpieza', 'Insumos varios de limpieza', 1, GETDATE(), 1);
GO


INSERT INTO catalogo.EstadoConsumible (NombreEstado, Descripcion, FechaCreacion, IdUsuarioCreacion)
VALUES
('Disponible', 'Consumible en existencia', GETDATE(), 1),
('Bajo Stock', 'Stock mínimo alcanzado', GETDATE(), 1),
('Agotado', 'Sin existencias', GETDATE(), 1);
GO


INSERT INTO catalogo.Pregunta (TextoPregunta, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES ('¿Cómo califica la atención recibida?', 1, GETDATE(), 1);
GO

INSERT INTO catalogo.Pregunta (TextoPregunta, IdEstadoGeneral, IdUsuarioCreacion)
VALUES ('¿Cómo califica la atención brindada por la mesa de servicio?', 1, 1);

INSERT INTO catalogo.Pregunta (TextoPregunta, IdEstadoGeneral, IdUsuarioCreacion)
VALUES ('¿El tiempo de respuesta a su solicitud fue adecuado?', 1, 1);

INSERT INTO catalogo.Pregunta (TextoPregunta, IdEstadoGeneral, IdUsuarioCreacion)
VALUES ('¿La solución entregada resolvió completamente su inconveniente?', 1, 1);

INSERT INTO catalogo.Pregunta (TextoPregunta, IdEstadoGeneral, IdUsuarioCreacion)
VALUES ('¿Qué tan satisfecho se encuentra con el servicio recibido?', 1, 1);

INSERT INTO catalogo.Respuesta (TextoRespuesta, ValorNumerico, IdEstadoGeneral, FechaCreacion, IdUsuarioCreacion)
VALUES
('Excelente', 5, 1, GETDATE(), 1),
('Bueno', 4, 1, GETDATE(), 1),
('Regular', 3, 1, GETDATE(), 1),
('Malo', 2, 1, GETDATE(), 1),
('Pésimo', 1, 1, GETDATE(), 1);
GO

-- PRIMARY KEYS - LLAVES PRIMARIAS


-- PRIMARY KEYS – ACCESO


ALTER TABLE acceso.Rol
ADD CONSTRAINT PKRol PRIMARY KEY (IdRol);
GO

ALTER TABLE acceso.Usuario
ADD CONSTRAINT PKUsuario PRIMARY KEY (IdUsuario);
GO




-- PRIMARY KEYS – CATÁLOGO


ALTER TABLE catalogo.EstadoGeneral
ADD CONSTRAINT PKEstadoGeneral PRIMARY KEY (IdEstadoGeneral);
GO

ALTER TABLE catalogo.AreaTecnica
ADD CONSTRAINT PKAreaTecnica PRIMARY KEY (IdAreaTecnica);
GO

ALTER TABLE catalogo.TipoTrabajo
ADD CONSTRAINT PKTipoTrabajo PRIMARY KEY (IdTipoTrabajo);
GO

ALTER TABLE catalogo.EstadoIntervencionTecnica
ADD CONSTRAINT PKEstadoIntervencion PRIMARY KEY (IdEstadoIntervencion);
GO

ALTER TABLE catalogo.EstadoCaso
ADD CONSTRAINT PKEstadoCaso PRIMARY KEY (IdEstadoCaso);
GO

ALTER TABLE catalogo.Prioridad
ADD CONSTRAINT PKPrioridad PRIMARY KEY (IdPrioridad);
GO

ALTER TABLE catalogo.TipoCaso
ADD CONSTRAINT PKTipoCaso PRIMARY KEY (IdTipoCaso);
GO

ALTER TABLE catalogo.CanalIngreso
ADD CONSTRAINT PKCanalIngreso PRIMARY KEY (IdCanalIngreso);
GO

ALTER TABLE catalogo.Pregunta
ADD CONSTRAINT PKPregunta PRIMARY KEY (IdPregunta);
GO

ALTER TABLE catalogo.Respuesta
ADD CONSTRAINT PKRespuesta PRIMARY KEY (IdRespuesta);
GO

ALTER TABLE catalogo.CategoriaActivo
ADD CONSTRAINT PKCategoriaActivo PRIMARY KEY (IdCategoriaActivo);
GO

ALTER TABLE catalogo.EstadoActivo
ADD CONSTRAINT PKEstadoActivo PRIMARY KEY (IdEstadoActivo);
GO

ALTER TABLE catalogo.TipoConsumible
ADD CONSTRAINT PKTipoConsumible PRIMARY KEY (IdTipoConsumible);
GO

ALTER TABLE catalogo.EstadoConsumible
ADD CONSTRAINT PKEstadoConsumible PRIMARY KEY (IdEstadoConsumible);
GO




-- PRIMARY KEYS – INVENTARIO


ALTER TABLE inventario.Ubicacion
ADD CONSTRAINT PKUbicacion PRIMARY KEY (IdUbicacion);
GO

ALTER TABLE inventario.Inventario
ADD CONSTRAINT PKInventario PRIMARY KEY (IdInventario);
GO

ALTER TABLE inventario.Activo
ADD CONSTRAINT PKActivo PRIMARY KEY (IdActivo);
GO

ALTER TABLE inventario.Componente
ADD CONSTRAINT PKComponente PRIMARY KEY (IdComponente);
GO

ALTER TABLE inventario.Consumible
ADD CONSTRAINT PKConsumible PRIMARY KEY (IdConsumible);
GO

ALTER TABLE inventario.HojaDeVidaActivo
ADD CONSTRAINT PKHojaDeVidaActivo PRIMARY KEY (IdHojaActivo);
GO




-- PRIMARY KEYS – SOPORTE
ALTER TABLE soporte.TrazabilidadCaso
ADD CONSTRAINT PKTrazabilidadCaso PRIMARY KEY CLUSTERED (IdTrazabilidadCaso);


ALTER TABLE soporte.Caso
ADD CONSTRAINT PKCaso PRIMARY KEY (IdCaso);
GO


ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT PKIntervencionTecnica PRIMARY KEY (IdIntervencionTecnica);
GO

ALTER TABLE soporte.DetalleCambioComponentes
ADD CONSTRAINT PKDetalleCambioComponentes PRIMARY KEY (IdCambioComponente);
GO

ALTER TABLE soporte.DetalleConsumible
ADD CONSTRAINT PKDetalleConsumible PRIMARY KEY (IdDetalleConsumible);
GO

ALTER TABLE soporte.RevisionAdmi
ADD CONSTRAINT PKRevisionAdmi PRIMARY KEY (IdRevisionAdmi);
GO

ALTER TABLE soporte.EncuestaCalidad
ADD CONSTRAINT PKEncuestaCalidad PRIMARY KEY (IdEncuesta);
GO

ALTER TABLE soporte.DetalleEncuesta
ADD CONSTRAINT PKDetalleEncuesta PRIMARY KEY (IdDetalleEncuesta);
GO


-- FOREIGN KEYS - LLAVES FORANEAS CON RESTRICCIONES


   -- FOREIGN KEYS – ACCESO
   

ALTER TABLE acceso.Rol
ADD CONSTRAINT FKRolEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE acceso.Usuario
ADD CONSTRAINT FKUsuarioRol
FOREIGN KEY (IdRol)
REFERENCES acceso.Rol(IdRol)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

   -- FOREIGN KEYS – CATÁLOGO
   

ALTER TABLE catalogo.AreaTecnica
ADD CONSTRAINT FKAreaTecnicaEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE catalogo.AreaTecnica
ADD CONSTRAINT FKAreaTecnicaEncargado
FOREIGN KEY (IdEncargado)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE inventario.Ubicacion
ADD CONSTRAINT FKUbicacionSede
FOREIGN KEY (IdSede)
REFERENCES catalogo.Sede(IdSede)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.TipoTrabajo
ADD CONSTRAINT FKTipoTrabajoEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.Prioridad
ADD CONSTRAINT FKPrioridadEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.TipoCaso
ADD CONSTRAINT FKTipoCasoEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.CanalIngreso
ADD CONSTRAINT FKCanalIngresoEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.Pregunta
ADD CONSTRAINT FKPreguntaEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE catalogo.Respuesta
ADD CONSTRAINT FKRespuestaEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.CategoriaActivo
ADD CONSTRAINT FKCategoriaActivoEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE catalogo.TipoConsumible
ADD CONSTRAINT FKTipoConsumibleEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


   -- FOREIGN KEYS – INVENTARIO
   

ALTER TABLE inventario.Inventario
ADD CONSTRAINT FKInventarioEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Inventario
ADD CONSTRAINT FKInventarioResponsable
FOREIGN KEY (IdResponsableInventario)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;




ALTER TABLE inventario.Activo
ADD CONSTRAINT FKActivoCategoriaActivo
FOREIGN KEY (IdCategoriaActivo)
REFERENCES catalogo.CategoriaActivo(IdCategoriaActivo)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Activo
ADD CONSTRAINT FKActivoEstadoActivo
FOREIGN KEY (IdEstadoActivo)
REFERENCES catalogo.EstadoActivo(IdEstadoActivo)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Activo
ADD CONSTRAINT FKActivoInventario
FOREIGN KEY (IdInventario)
REFERENCES inventario.Inventario(IdInventario)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Activo
ADD CONSTRAINT FKActivoUbicacion
FOREIGN KEY (IdUbicacion)
REFERENCES inventario.Ubicacion(IdUbicacion)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Activo
ADD CONSTRAINT FKActivoResponsable
FOREIGN KEY (IdResponsableActivo)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE inventario.HojaDeVidaActivo
ADD CONSTRAINT FKHojaVidaActivo
FOREIGN KEY (IdActivo)
REFERENCES inventario.Activo(IdActivo)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE inventario.HojaDeVidaActivo
ADD CONSTRAINT FKHojaDeVidaActivoCaso
FOREIGN KEY (IdCaso)
REFERENCES soporte.Caso(IdCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE inventario.Componente
ADD CONSTRAINT FKComponenteInventario
FOREIGN KEY (IdInventario)
REFERENCES inventario.Inventario(IdInventario)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE inventario.Consumible
ADD CONSTRAINT FKConsumibleInventario
FOREIGN KEY (IdInventario)
REFERENCES inventario.Inventario(IdInventario)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Consumible
ADD CONSTRAINT FKConsumibleTipo
FOREIGN KEY (IdTipoConsumible)
REFERENCES catalogo.TipoConsumible(IdTipoConsumible)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE inventario.Consumible
ADD CONSTRAINT FKConsumibleEstadoConsumible
FOREIGN KEY (IdEstadoConsumible)
REFERENCES catalogo.EstadoConsumible(IdEstadoConsumible)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


ALTER TABLE inventario.Componente
ADD CONSTRAINT FKComponenteEstadoGeneral
FOREIGN KEY (IdEstadoGeneral)
REFERENCES catalogo.EstadoGeneral(IdEstadoGeneral)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


   -- FOREIGN KEYS – SOPORTE (CASO, TRAZABILIDAD, INTERVENCIÓN)
   

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoEstadoCaso
FOREIGN KEY (IdEstadoCaso)
REFERENCES catalogo.EstadoCaso(IdEstadoCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoUsuarioReporta
FOREIGN KEY (IdUsuarioReporta)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoTipoCaso
FOREIGN KEY (IdTipoCaso)
REFERENCES catalogo.TipoCaso(IdTipoCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoActivo
FOREIGN KEY (IdActivo)
REFERENCES inventario.Activo(IdActivo)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoAreaTecnica
FOREIGN KEY (IdAreaTecnica)
REFERENCES catalogo.AreaTecnica(IdAreaTecnica)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoPrioridad
FOREIGN KEY (IdPrioridad)
REFERENCES catalogo.Prioridad(IdPrioridad)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoCanalIngreso
FOREIGN KEY (IdCanalIngreso)
REFERENCES catalogo.CanalIngreso(IdCanalIngreso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.Caso
ADD CONSTRAINT FKCasoTecnicoAsignado
FOREIGN KEY (IdTecnicoAsignado)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


   -- TRAZABILIDAD
   

ALTER TABLE soporte.TrazabilidadCaso
ADD CONSTRAINT FKTrazabilidadCaso
FOREIGN KEY (IdCaso)
REFERENCES soporte.Caso(IdCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.TrazabilidadCaso
ADD CONSTRAINT FKTrazabilidadUsuarioAccion
FOREIGN KEY (IdUsuarioAccion)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.TrazabilidadCaso
ADD CONSTRAINT FKTrazabilidadEstadoCaso
FOREIGN KEY (IdEstadoCaso)
REFERENCES catalogo.EstadoCaso(IdEstadoCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.TrazabilidadCaso
ADD CONSTRAINT FKTrazabilidadAreaTecnica
FOREIGN KEY (IdAreaTecnica)
REFERENCES catalogo.AreaTecnica(IdAreaTecnica)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.TrazabilidadCaso
ADD CONSTRAINT FKTrazabilidadTecnicoAsignado
FOREIGN KEY (IdTecnicoAsignado)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


   -- INTERVENCIÓN TÉCNICA
   

ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT FKIntervencionTrazabilidad
FOREIGN KEY (IdTrazabilidadCaso)
REFERENCES soporte.TrazabilidadCaso(IdTrazabilidadCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT FKIntervencionTipoTrabajo
FOREIGN KEY (IdTipoTrabajo)
REFERENCES catalogo.TipoTrabajo(IdTipoTrabajo)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT FKIntervencionEstadoIntervencion
FOREIGN KEY (IdEstadoIntervencion)
REFERENCES catalogo.EstadoIntervencionTecnica(IdEstadoIntervencion)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT FKIntervencionUsuarioAccion
FOREIGN KEY (IdUsuarioAccion)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;




   -- DETALLE CAMBIO COMPONENTES
   

ALTER TABLE soporte.DetalleCambioComponentes
ADD CONSTRAINT FKCambioComponentesIntervencion
FOREIGN KEY (IdIntervencionTecnica)
REFERENCES soporte.IntervencionTecnica(IdIntervencionTecnica)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.DetalleCambioComponentes
ADD CONSTRAINT FKCambioComponentesComponente
FOREIGN KEY (IdComponente)
REFERENCES inventario.Componente(IdComponente)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


  -- DETALLE CONSUMIBLES
   

ALTER TABLE soporte.DetalleConsumible
ADD CONSTRAINT FKDetalleConsumibleIntervencion
FOREIGN KEY (IdIntervencionTecnica)
REFERENCES soporte.IntervencionTecnica(IdIntervencionTecnica)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.DetalleConsumible
ADD CONSTRAINT FKDetalleConsumibleConsumible
FOREIGN KEY (IdConsumible)
REFERENCES inventario.Consumible(IdConsumible)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


   -- REVISIÓN ADMINISTRATIVA
   

ALTER TABLE soporte.RevisionAdmi
ADD CONSTRAINT FKRevisionAdmiIntervencion
FOREIGN KEY (IdIntervencionTecnica)
REFERENCES soporte.IntervencionTecnica(IdIntervencionTecnica)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.RevisionAdmi
ADD CONSTRAINT FKRevisionAdmiUsuario
FOREIGN KEY (IdUsuarioCreacion)
REFERENCES acceso.Usuario(IdUsuario)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


   -- ENCUESTAS  

ALTER TABLE soporte.EncuestaCalidad
ADD CONSTRAINT FKEncuestaCaso
FOREIGN KEY (IdCaso)
REFERENCES soporte.Caso(IdCaso)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.DetalleEncuesta
ADD CONSTRAINT FKDetalleEncuestaEncuesta
FOREIGN KEY (IdEncuesta)
REFERENCES soporte.EncuestaCalidad(IdEncuesta)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.DetalleEncuesta
ADD CONSTRAINT FKDetalleEncuestaPregunta
FOREIGN KEY (IdPregunta)
REFERENCES catalogo.Pregunta(IdPregunta)
ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE soporte.DetalleEncuesta
ADD CONSTRAINT FKDetalleEncuestaRespuesta
FOREIGN KEY (IdRespuesta)
REFERENCES catalogo.Respuesta(IdRespuesta)
ON DELETE NO ACTION ON UPDATE NO ACTION;
GO


-- CHECKS

ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT   CKFechasIntervencionTecnica
CHECK (FechaFin IS NULL OR FechaFin >= FechaInicio);
GO

ALTER TABLE soporte.IntervencionTecnica
ADD CONSTRAINT CKIntervencionFechas
CHECK (FechaFin IS NULL OR FechaFin >= FechaInicio);

GO


ALTER TABLE soporte.DetalleCambioComponentes
ADD CONSTRAINT CKCambioComponentesCantidad
CHECK (Cantidad > 0);
GO

ALTER TABLE soporte.DetalleConsumible
ADD CONSTRAINT CKConsumibleCantidad
CHECK (Cantidad > 0);
GO


ALTER TABLE inventario.Componente
ADD CONSTRAINT CKComponenteStockMinimo
CHECK (StockMinimo >= 0);
GO



ALTER TABLE inventario.Consumible
ADD CONSTRAINT CKConsumibleStockValidacion
CHECK (
    StockMinimo >= 0
    AND StockActual >= 0
);
GO


ALTER TABLE catalogo.Prioridad
ADD CONSTRAINT CKPrioridadSLA
CHECK (    TiempoRespuestaDias > 0
    AND TiempoResolucionDias > 0
    AND TiempoResolucionDias >= TiempoRespuestaDias);
GO



ALTER TABLE soporte.Caso
ADD CONSTRAINT CKCasoTelefono
CHECK (
    TelefonoContacto IS NULL
    OR (
        TelefonoContacto NOT LIKE '%[^0-9+]%'  
        AND LEN(TelefonoContacto) BETWEEN 10 AND 11
        AND (
            TelefonoContacto NOT LIKE '%+%'     
            OR TelefonoContacto LIKE '+%'       
        )
    )
);
GO


ALTER TABLE acceso.Usuario
ADD CONSTRAINT CKUsuarioEmail
CHECK (Email LIKE '%_@_%._%');
GO


ALTER TABLE soporte.Caso
ADD CONSTRAINT CKCasoFechas
CHECK (
    (FechaAceptacion IS NULL OR FechaAceptacion >= FechaRegistro) AND
    (FechaResolucion IS NULL OR FechaResolucion >= FechaAceptacion) AND
    (FechaCierre IS NULL OR FechaCierre >= FechaResolucion)
);
GO


-- UNIQUE CONSTRAINTS 
---- Vamos a validar la posuble duplicidad en el sistema sin necesidad
---- del backend.

-- ACCESO
ALTER TABLE acceso.Usuario
ADD CONSTRAINT UQUsuarioEmail UNIQUE (Email);

ALTER TABLE acceso.Rol
ADD CONSTRAINT UQRolNombreRol UNIQUE (NombreRol);

-- CATALOGO

ALTER TABLE catalogo.EstadoActivo
ADD CONSTRAINT UQEstadoActivoNombre UNIQUE (NombreEstado);

ALTER TABLE catalogo.AreaTecnica
ADD CONSTRAINT UQAreaTecnicaNombre UNIQUE (NombreAreaTecnica);

ALTER TABLE catalogo.TipoTrabajo
ADD CONSTRAINT UQTipoTrabajoNombre UNIQUE (NombreTipoTrabajo);

ALTER TABLE catalogo.EstadoIntervencionTecnica
ADD CONSTRAINT UQEstadoIntervencionNombre UNIQUE (NombreEstado);

ALTER TABLE catalogo.EstadoCaso
ADD CONSTRAINT UQEstadoCasoNombre UNIQUE (NombreEstadoCaso);

ALTER TABLE catalogo.Prioridad
ADD CONSTRAINT UQPrioridadNombre UNIQUE (NombrePrioridad);

ALTER TABLE catalogo.TipoCaso
ADD CONSTRAINT UQTipoCasoNombre UNIQUE (NombreTipoCaso);

ALTER TABLE catalogo.CanalIngreso
ADD CONSTRAINT UQCanalIngresoNombre UNIQUE (NombreCanal);

ALTER TABLE catalogo.Pregunta
ADD CONSTRAINT UQPreguntaTexto UNIQUE (TextoPregunta);

ALTER TABLE catalogo.Respuesta
ADD CONSTRAINT UQRespuestaTexto UNIQUE (TextoRespuesta);

ALTER TABLE catalogo.CategoriaActivo
ADD CONSTRAINT UQCategoriaActivoNombre UNIQUE (NombreCategoria);

ALTER TABLE catalogo.EstadoGeneral
ADD CONSTRAINT UQEstadoGeneralNombre UNIQUE (NombreEstado);

-- INVENTARIO
ALTER TABLE inventario.Componente
ADD CONSTRAINT UQComponenteNombre UNIQUE (NombreComponente);

ALTER TABLE inventario.Consumible
ADD CONSTRAINT UQConsumibleClave UNIQUE (NombreConsumible, Marca, Modelo);

ALTER TABLE inventario.Activo
ADD CONSTRAINT UQActivoCodigoPatrimonial UNIQUE (CodigoPatrimonial);
GO


   -- ÍNDICES CRÍTICOS PARA RENDIMIENTO – MÓDULO ACCESO

-- Usuario consulta mucho por Rol
CREATE INDEX IXUsuarioRol ON acceso.Usuario(IdRol);
GO

CREATE INDEX IXRolEstadoGeneral ON acceso.Rol(IdEstadoGeneral);
GO

CREATE UNIQUE INDEX UXCasoNumeroCaso ON soporte.Caso (NumeroCaso);
GO

  --  ÍNDICES – MÓDULO CATÁLOGO
  
CREATE INDEX IXAreaTecnicaEstado ON catalogo.AreaTecnica(IdEstadoGeneral);
GO

CREATE INDEX IXTipoTrabajoEstado ON catalogo.TipoTrabajo(IdEstadoGeneral);
GO

CREATE INDEX IXPrioridadEstado ON catalogo.Prioridad(IdEstadoGeneral);
GO

CREATE INDEX IXTipoCasoEstado ON catalogo.TipoCaso(IdEstadoGeneral);
GO

CREATE INDEX IXCanalIngresoEstado ON catalogo.CanalIngreso(IdEstadoGeneral);
GO

CREATE INDEX IXPreguntaEstado ON catalogo.Pregunta(IdEstadoGeneral);
GO

CREATE INDEX IXRespuestaEstado ON catalogo.Respuesta(IdEstadoGeneral);
GO

CREATE INDEX IXCategoriaActivoEstado ON catalogo.CategoriaActivo(IdEstadoGeneral);
GO

CREATE INDEX IXTipoConsumibleEstado ON catalogo.TipoConsumible(IdEstadoGeneral);
GO


 --   ÍNDICES – INVENTARIO.ACTIVO


-- Búsqueda por inventario padre
CREATE INDEX IXActivoInventario ON inventario.Activo(IdInventario);
GO

-- Búsqueda por ubicación física
CREATE INDEX IXActivoUbicacion ON inventario.Activo(IdUbicacion);
GO

-- Búsqueda por estado del activo
CREATE INDEX IXActivoEstado ON inventario.Activo(IdEstadoActivo);
GO

-- Búsqueda por categoría de activo
CREATE INDEX IXActivoCategoria ON inventario.Activo(IdCategoriaActivo);
GO

 --   ÍNDICES – INVENTARIO.CONSUMIBLE


-- Filtros por inventario
CREATE INDEX IXConsumibleInventario ON inventario.Consumible(IdInventario);
GO

-- Filtros por tipo de consumible
CREATE INDEX IXConsumibleTipo ON inventario.Consumible(IdTipoConsumible);
GO

-- Filtros por estado de consumible
CREATE INDEX IXConsumibleEstado ON inventario.Consumible(IdEstadoConsumible);
GO

-- Alertas de stock bajo
CREATE INDEX IXConsumibleStockActual ON inventario.Consumible(StockActual);
GO

  --  ÍNDICES – INVENTARIO.HojaDeVidaActivo


-- Historial por activo
CREATE INDEX IXHojaVidaActivo ON inventario.HojaDeVidaActivo(IdActivo);
GO

-- Consultas por fecha
CREATE INDEX IXHojaVidaFechaRegistro ON inventario.HojaDeVidaActivo(FechaRegistro);
GO



 --   ÍNDICES – SOPORTE.CASO


-- Casos filtrados por estado
CREATE INDEX IXCasoEstado ON soporte.Caso(IdEstadoCaso);
GO

-- Filtrados por área técnica
CREATE INDEX IXCasoAreaTecnica ON soporte.Caso(IdAreaTecnica);
GO

-- Filtrados por técnico asignado
CREATE INDEX IXCasoTecnicoAsignado ON soporte.Caso(IdTecnicoAsignado);
GO

-- Filtrado por prioridad
CREATE INDEX IXCasoPrioridad ON soporte.Caso(IdPrioridad);
GO

-- Filtrado por tipo de caso
CREATE INDEX IXCasoTipoCaso ON soporte.Caso(IdTipoCaso);
GO

-- Filtrado por fecha de registro (reportes)
CREATE INDEX IXCasoFechaRegistro ON soporte.Caso(FechaRegistro);
GO

 --   ÍNDICES – SOPORTE.TRAZABILIDADCASO

-- Consultas por caso
CREATE INDEX IXTrazabilidadCaso ON soporte.TrazabilidadCaso(IdCaso);
GO

-- Consultas por fecha
CREATE INDEX IXTrazabilidadFechaEvento ON soporte.TrazabilidadCaso(FechaEvento);
GO

-- Consultas por usuario que ejecutó el cambio
CREATE INDEX IXTrazabilidadUsuario ON soporte.TrazabilidadCaso(IdUsuarioAccion);
GO

--    ÍNDICES – SOPORTE.INTERVENCIONTECNICA

-- Relación directa a trazabilidad
CREATE INDEX IXIntervencionTrazabilidad 
ON soporte.IntervencionTecnica(IdTrazabilidadCaso);
GO

-- Filtros por técnico
CREATE INDEX IXIntervencionUsuario 
ON soporte.IntervencionTecnica(IdUsuarioAccion);
GO

--    ÍNDICES – SOPORTE.DETALLECONSUMIBLE

CREATE INDEX IXDetalleConsumibleIntervencion 
ON soporte.DetalleConsumible(IdIntervencionTecnica);
GO

 --   ÍNDICES – SOPORTE.DETALLECAMBIOCOMPONENTES

CREATE INDEX IXDetalleCambioIntervencion 
ON soporte.DetalleCambioComponentes(IdIntervencionTecnica);
GO

CREATE INDEX IXIntervencionEstado
ON soporte.IntervencionTecnica (IdEstadoIntervencion);
GO

CREATE INDEX IXCasoUsuarioReporta
ON soporte.Caso (IdUsuarioReporta);
GO


CREATE INDEX IXCasoCanalIngreso
ON soporte.Caso (IdCanalIngreso);
GO


CREATE INDEX IXHojaVidaTipoEvento
ON inventario.HojaDeVidaActivo (TipoEvento);
GO


CREATE INDEX IXHojaVidaUsuario
ON inventario.HojaDeVidaActivo (IdUsuarioCreacion);
GO


CREATE INDEX IXTrazabilidadTipoEvento
ON soporte.TrazabilidadCaso (TipoEvento);
GO


CREATE INDEX IXTrazabilidadEstado
ON soporte.TrazabilidadCaso (IdEstadoCaso);
GO
CREATE INDEX IXComponenteInventario
ON inventario.Componente(IdInventario);
GO

CREATE INDEX IXComponenteStockMinimo
ON inventario.Componente(StockMinimo);
GO


INSERT INTO catalogo.EstadoCaso (NombreEstadoCaso, DescripcionEstadoCaso, FechaCreacion, IdUsuarioCreacion)
    VALUES ('Escalado', 'El caso fue escalado a otra área técnica', GETDATE(), 1);
