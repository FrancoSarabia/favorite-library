## 🚀 Instrucciones del proyecto

### Requisitos
- .NET SDK 10.0  
  https://dotnet.microsoft.com/download

### Clonar repositorio
```bash
git clone https://github.com/FrancoSarabia/favorite-library.git
```

### Ejecutar Tests

1. Desde la carpeta del proyecto de tests:

```bash
cd FavoriteLibrary/FavoriteLibrary.Tests
```
o desde la raiz del proyecto ejecutar el siguiente comando:
```bash
dotnet test
```
dando como resultado lo siguiente:
```bash
Test summary: total: 6, failed: 0, succeeded: 6, skipped: 0,
```

## 🗄️ Configuración de Base de Datos

El proyecto utiliza SQL Server local.  
Se incluye un script DDL para crear la base de datos, tablas y datos de ejemplo (seed).

### Ejecutar el script de base de datos

El archivo se puede ejecutar en:

#### Opción 1 – SQL Server Management Studio (SSMS)

1. Abrir SQL Server Management Studio
2. Conectarse a `localhost\SQLEXPRESS`
3. Abrir el archivo `database/init.sql`
4. Ejecutar el script presionando **F5**

#### Opción 2 – Línea de comandos (sqlcmd)

Desde la raíz del proyecto ejecutar:

```bash
sqlcmd -S localhost\SQLEXPRESS -E -i database/init.sql
```
### 🔌 Connection String

El archivo appsettings.json contiene el siguiente connection string:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FavoriteLibraryDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Host del proyecto

Por defecto el proyecto de angular es `http://localhost:4200`, pero si se usa otro host modificar esta instrucción en el appsettings.json:

```bash
"CorsSettings": {
  "AllowedOrigins": [ "http://localhost:4200" ]
}
```