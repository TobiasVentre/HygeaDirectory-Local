# HygeaDirectory-Local

## Configuración de persistencia con SQL Server (MSSQL LocalDB)

Se implementó persistencia con **Entity Framework Core + SQL Server**, usando el connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=directory;Trusted_Connection=True;TrustServerCertificate=true;"
}
```

## ¿Qué quedó configurado?

- `DirectoryDbContext` con `DbSet<User>`.
- Configuración de entidad `User` (`Users` table, longitudes, índice único por email).
- Repositorio `UserRepository` contra EF Core.
- Registro de infraestructura con `AddInfrastructure(...)`.
- Migraciones iniciales en `src/DirectoryMS.Infrastructure/Persistence/Migrations`.
- Ejecución automática de `Database.Migrate()` al iniciar la API.

## Paso a paso para ejecutar todo

> Requisito: tener instalado **.NET SDK 9** y acceso a `MSSQLLocalDB` en Windows.

1. Restaurar dependencias:
   ```bash
   dotnet restore HygeaDirectory.sln
   ```

2. (Opcional) Compilar solución:
   ```bash
   dotnet build HygeaDirectory.sln
   ```

3. Aplicar migraciones manualmente (si querés ejecutarlo explícitamente):
   ```bash
   dotnet ef database update --project src/DirectoryMS.Infrastructure --startup-project src/DirectoryMS
   ```

4. Levantar la API:
   ```bash
   dotnet run --project src/DirectoryMS
   ```

5. Probar Swagger:
   - Abrir: `https://localhost:xxxx/swagger` (el puerto exacto lo muestra la consola al correr).

## Cómo generar nuevas migraciones

Cuando cambies el modelo:

```bash
dotnet ef migrations add <NombreMigracion> --project src/DirectoryMS.Infrastructure --startup-project src/DirectoryMS --output-dir Persistence/Migrations
```

Luego aplicarlas:

```bash
dotnet ef database update --project src/DirectoryMS.Infrastructure --startup-project src/DirectoryMS
```
