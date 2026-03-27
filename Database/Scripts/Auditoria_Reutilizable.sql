/*
    999_Auditoria_Reutilizable.sql
    --------------------------------------------
    Objetivo:
    1) Crear schema auditoria
    2) Crear UNA tabla de auditoria por cada tabla funcional
       (acceso, catalogo, inventario, soporte)
    3) Crear procedimientos reutilizables para Insert/Update/Delete
    4) Crear triggers automaticamente para todas las tablas auditables

    Nota:
    - Este enfoque deja la logica central en procedimientos reutilizables.
    - Cada tabla tiene un trigger liviano y su tabla de auditoria dedicada.
    - Se registran snapshots JSON de filas nuevas/anteriores.
*/

SET NOCOUNT ON;


/* ============================================================
   1) Schema de auditoria
   ============================================================ */
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'auditoria')
BEGIN
    EXEC ('CREATE SCHEMA auditoria');
END;


/* ============================================================
   2) Crear tablas de auditoria por cada tabla funcional
   Formato: auditoria.<Schema>_<Tabla>_Audit
   ============================================================ */
BEGIN
    DECLARE
        @SourceSchema SYSNAME,
        @SourceTable SYSNAME,
        @CreateAuditTableSql NVARCHAR(MAX);

    DECLARE AuditTablesCursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT
        s.name AS SourceSchema,
        t.name AS SourceTable
    FROM sys.tables t
    INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
    WHERE t.is_ms_shipped = 0
      AND s.name IN ('acceso', 'catalogo', 'inventario', 'soporte')
      AND s.name <> 'auditoria'
      AND t.name NOT LIKE '%[_]Audit'
    ORDER BY s.name, t.name;

    OPEN AuditTablesCursor;
    FETCH NEXT FROM AuditTablesCursor INTO @SourceSchema, @SourceTable;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @CreateAuditTableSql = N'
    IF OBJECT_ID(N''auditoria.' + @SourceSchema + N'_' + @SourceTable + N'_Audit'', N''U'') IS NULL
    BEGIN
        CREATE TABLE auditoria.' + QUOTENAME(@SourceSchema + N'_' + @SourceTable + N'_Audit') + N'(
            AuditId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            ActionType CHAR(1) NOT NULL CHECK (ActionType IN (''I'', ''U'', ''D'')),
            EventDateUtc DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
            ChangedBy NVARCHAR(256) NULL,
            SessionUserId BIGINT NULL,
            HostName NVARCHAR(128) NULL,
            AppName NVARCHAR(128) NULL,
            CorrelationId NVARCHAR(100) NULL,
            PrimaryKeyJson NVARCHAR(MAX) NULL,
            OldValuesJson NVARCHAR(MAX) NULL,
            NewValuesJson NVARCHAR(MAX) NULL
        );
    END;';

        EXEC sys.sp_executesql @CreateAuditTableSql;

        FETCH NEXT FROM AuditTablesCursor INTO @SourceSchema, @SourceTable;
    END;

    CLOSE AuditTablesCursor;
    DEALLOCATE AuditTablesCursor;
END;

/* ============================================================
   3) Procedimientos reutilizables de auditoria
   ============================================================ */
CREATE OR ALTER PROCEDURE auditoria.sp_AuditoriaRegistrar
    @SchemaName SYSNAME,
    @TableName SYSNAME,
    @ActionType CHAR(1),
    @PrimaryKeyJson NVARCHAR(MAX) = NULL,
    @OldValuesJson NVARCHAR(MAX) = NULL,
    @NewValuesJson NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @ActionType NOT IN ('I', 'U', 'D')
    BEGIN
        THROW 50001, 'ActionType invalido. Use I, U o D.', 1;
    END;

    DECLARE @AuditTable SYSNAME = @SchemaName + N'_' + @TableName + N'_Audit';

    IF OBJECT_ID(N'auditoria.' + @AuditTable, N'U') IS NULL
    BEGIN
        THROW 50002, 'No existe tabla de auditoria para la tabla origen.', 1;
    END;

    DECLARE @ChangedBy NVARCHAR(256) =
        COALESCE(TRY_CAST(SESSION_CONTEXT(N'username') AS NVARCHAR(256)), SUSER_SNAME());
    DECLARE @SessionUserId BIGINT = TRY_CAST(SESSION_CONTEXT(N'user_id') AS BIGINT);
    DECLARE @CorrelationId NVARCHAR(100) = TRY_CAST(SESSION_CONTEXT(N'correlation_id') AS NVARCHAR(100));

    DECLARE @Sql NVARCHAR(MAX) = N'
        INSERT INTO auditoria.' + QUOTENAME(@AuditTable) + N'
        (
            ActionType,
            ChangedBy,
            SessionUserId,
            HostName,
            AppName,
            CorrelationId,
            PrimaryKeyJson,
            OldValuesJson,
            NewValuesJson
        )
        VALUES
        (
            @ActionType,
            @ChangedBy,
            @SessionUserId,
            HOST_NAME(),
            APP_NAME(),
            @CorrelationId,
            @PrimaryKeyJson,
            @OldValuesJson,
            @NewValuesJson
        );';

    EXEC sys.sp_executesql
        @Sql,
        N'@ActionType CHAR(1),
          @ChangedBy NVARCHAR(256),
          @SessionUserId BIGINT,
          @CorrelationId NVARCHAR(100),
          @PrimaryKeyJson NVARCHAR(MAX),
          @OldValuesJson NVARCHAR(MAX),
          @NewValuesJson NVARCHAR(MAX)',
        @ActionType = @ActionType,
        @ChangedBy = @ChangedBy,
        @SessionUserId = @SessionUserId,
        @CorrelationId = @CorrelationId,
        @PrimaryKeyJson = @PrimaryKeyJson,
        @OldValuesJson = @OldValuesJson,
        @NewValuesJson = @NewValuesJson;
END;

CREATE OR ALTER PROCEDURE auditoria.sp_AuditoriaInsertar
    @SchemaName SYSNAME,
    @TableName SYSNAME,
    @PrimaryKeyJson NVARCHAR(MAX) = NULL,
    @NewValuesJson NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    EXEC auditoria.sp_AuditoriaRegistrar
        @SchemaName = @SchemaName,
        @TableName = @TableName,
        @ActionType = 'I',
        @PrimaryKeyJson = @PrimaryKeyJson,
        @OldValuesJson = NULL,
        @NewValuesJson = @NewValuesJson;
END;

CREATE OR ALTER PROCEDURE auditoria.sp_AuditoriaActualizar
    @SchemaName SYSNAME,
    @TableName SYSNAME,
    @PrimaryKeyJson NVARCHAR(MAX) = NULL,
    @OldValuesJson NVARCHAR(MAX) = NULL,
    @NewValuesJson NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    EXEC auditoria.sp_AuditoriaRegistrar
        @SchemaName = @SchemaName,
        @TableName = @TableName,
        @ActionType = 'U',
        @PrimaryKeyJson = @PrimaryKeyJson,
        @OldValuesJson = @OldValuesJson,
        @NewValuesJson = @NewValuesJson;
END;

CREATE OR ALTER PROCEDURE auditoria.sp_AuditoriaEliminar
    @SchemaName SYSNAME,
    @TableName SYSNAME,
    @PrimaryKeyJson NVARCHAR(MAX) = NULL,
    @OldValuesJson NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    EXEC auditoria.sp_AuditoriaRegistrar
        @SchemaName = @SchemaName,
        @TableName = @TableName,
        @ActionType = 'D',
        @PrimaryKeyJson = @PrimaryKeyJson,
        @OldValuesJson = @OldValuesJson,
        @NewValuesJson = NULL;
END;

/* ============================================================
   4) Generador de triggers para TODAS las tablas auditables
   - Crea 1 trigger por tabla
   - Maneja I/U/D
   - Para UPDATE, ignora updates sin cambios reales
   ============================================================ */
BEGIN
    DECLARE
        @SchemaName SYSNAME,
        @TableName SYSNAME,
        @PkProjection NVARCHAR(MAX),
        @TriggerSql NVARCHAR(MAX);

    DECLARE TriggerCursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT
        s.name AS SchemaName,
        t.name AS TableName,
        (
            SELECT STRING_AGG(N'SRC.' + QUOTENAME(c.name), N', ')
            FROM sys.indexes i
            INNER JOIN sys.index_columns ic
                ON ic.object_id = i.object_id
               AND ic.index_id = i.index_id
            INNER JOIN sys.columns c
                ON c.object_id = ic.object_id
               AND c.column_id = ic.column_id
            WHERE i.object_id = t.object_id
              AND i.is_primary_key = 1
        ) AS PkProjection
    FROM sys.tables t
    INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
    WHERE t.is_ms_shipped = 0
      AND s.name IN ('acceso', 'catalogo', 'inventario', 'soporte')
      AND s.name <> 'auditoria'
      AND t.name NOT LIKE '%[_]Audit';

    OPEN TriggerCursor;
    FETCH NEXT FROM TriggerCursor INTO @SchemaName, @TableName, @PkProjection;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @PkProjection IS NULL OR LTRIM(RTRIM(@PkProjection)) = N''
        BEGIN
            SET @PkProjection = N'NULL AS NoPrimaryKey';
        END;

        SET @TriggerSql = N'
    CREATE OR ALTER TRIGGER ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(N'trg_AUD_' + @TableName) + N'
    ON ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
    AFTER INSERT, UPDATE, DELETE
    AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @ActionType CHAR(1);
        DECLARE @PrimaryKeyJson NVARCHAR(MAX);
        DECLARE @OldValuesJson NVARCHAR(MAX) = NULL;
        DECLARE @NewValuesJson NVARCHAR(MAX) = NULL;

        IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
            SET @ActionType = ''U'';
        ELSE IF EXISTS (SELECT 1 FROM inserted)
            SET @ActionType = ''I'';
        ELSE
            SET @ActionType = ''D'';

        IF @ActionType = ''U''
           AND NOT EXISTS (SELECT * FROM inserted EXCEPT SELECT * FROM deleted)
           AND NOT EXISTS (SELECT * FROM deleted EXCEPT SELECT * FROM inserted)
        BEGIN
            RETURN;
        END;

        IF @ActionType = ''D''
            SELECT @PrimaryKeyJson = (SELECT ' + @PkProjection + N' FROM deleted SRC FOR JSON PATH, INCLUDE_NULL_VALUES);
        ELSE
            SELECT @PrimaryKeyJson = (SELECT ' + @PkProjection + N' FROM inserted SRC FOR JSON PATH, INCLUDE_NULL_VALUES);

        IF @ActionType IN (''U'', ''D'')
            SELECT @OldValuesJson = (SELECT * FROM deleted FOR JSON PATH, INCLUDE_NULL_VALUES);

        IF @ActionType IN (''I'', ''U'')
            SELECT @NewValuesJson = (SELECT * FROM inserted FOR JSON PATH, INCLUDE_NULL_VALUES);

        IF @ActionType = ''I''
            EXEC auditoria.sp_AuditoriaInsertar
                @SchemaName = N''' + @SchemaName + N''',
                @TableName = N''' + @TableName + N''',
                @PrimaryKeyJson = @PrimaryKeyJson,
                @NewValuesJson = @NewValuesJson;
        ELSE IF @ActionType = ''U''
            EXEC auditoria.sp_AuditoriaActualizar
                @SchemaName = N''' + @SchemaName + N''',
                @TableName = N''' + @TableName + N''',
                @PrimaryKeyJson = @PrimaryKeyJson,
                @OldValuesJson = @OldValuesJson,
                @NewValuesJson = @NewValuesJson;
        ELSE
            EXEC auditoria.sp_AuditoriaEliminar
                @SchemaName = N''' + @SchemaName + N''',
                @TableName = N''' + @TableName + N''',
                @PrimaryKeyJson = @PrimaryKeyJson,
                @OldValuesJson = @OldValuesJson;
    END;';

        EXEC sys.sp_executesql @TriggerSql;

        FETCH NEXT FROM TriggerCursor INTO @SchemaName, @TableName, @PkProjection;
    END;

    CLOSE TriggerCursor;
    DEALLOCATE TriggerCursor;
END;


/* ============================================================
   5) Ejemplo real de como queda una tabla y trigger
   ============================================================

   Tabla de auditoria del caso:
   auditoria.soporte_Caso_Audit

   Trigger generado:
   soporte.trg_AUD_Caso

   Consulta de ejemplo:
   SELECT TOP 100 *
   FROM auditoria.soporte_Caso_Audit
   ORDER BY AuditId DESC;
*/
