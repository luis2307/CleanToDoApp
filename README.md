# Configuración de la Aplicación .NET para Ejecutar con SQL Server en Docker

Este documento proporciona una guía detallada para configurar una aplicación .NET para que funcione con una base de datos SQL Server en Docker. A continuación, se describen los pasos necesarios para configurar y ejecutar la base de datos, así como actualizar la cadena de conexión en la aplicación .NET.

## Paso 1: Configuración de SQL Server en Docker

Primero, necesitamos configurar un contenedor de Docker que ejecute SQL Server. Para esto, utilizamos el siguiente comando:

```sh
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<YourPassword>' -p 1433:1433 --name sqlserver -v sqlserver_data:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2022-latest
```

- ACCEPT_EULA=Y: Acepta los términos de licencia de SQL Server.
- SA_PASSWORD=<YourPassword>: Establece la contraseña del administrador del sistema (SA).
- -p 1433:1433: Mapea el puerto 1433 del contenedor al puerto 1433 de la máquina host.
- --name sqlserver: Asigna el nombre sqlserver al contenedor.
- -v sqlserver_data:/var/opt/mssql: Crea un volumen para persistir los datos de SQL Server.
- -d mcr.microsoft.com/mssql/server:2022-latest: Especifica la imagen de SQL Server 2022.

## Paso 2: Crear la Base de Datos "ToDo"

Después de iniciar el contenedor de Docker con SQL Server, necesitamos crear una base de datos llamada `ToDo`. Esto se puede hacer utilizando SQL Server Management Studio (SSMS) o cualquier otra herramienta de administración de SQL Server. Aquí se muestra un ejemplo de cómo crear la base de datos utilizando un script SQL:

```sql
CREATE DATABASE ToDo;
```
Este comando se puede ejecutar en SSMS o cualquier cliente SQL compatible.

## Paso 3: Actualizar la Cadena de Conexión en appsettings.json

Una vez que la base de datos esté creada, debemos actualizar la cadena de conexión en el archivo `appsettings.json` de nuestra aplicación .NET. La configuración del archivo debería parecerse a la siguiente:
```json
{
  "ConnectionStrings": {
    "sqlConnection": "Server=[Server],1433;Database=ToDo;User Id=[user];Password=[Password];Trust Server Certificate=true"
  }
} 
```

Reemplace los siguientes marcadores de posición con los valores adecuados:
- `[Server]`: La dirección del servidor SQL (usualmente localhost si está ejecutando Docker en su máquina local).
- `[user]`: El nombre de usuario de SQL Server (usualmente sa para el administrador del sistema).
- `[Password]`: La contraseña del usuario configurado.
Por ejemplo, si está ejecutando Docker en su máquina local y ha establecido la contraseña `MyStrongPassword`, la cadena de conexión se vería así:

```json
{
  "ConnectionStrings": {
    "sqlConnection": "Server=localhost,1433;Database=ToDo;User Id=sa;Password=MyStrongPassword;Trust Server Certificate=true"
  }
}
```

## Paso 4: Configuración de Redis en Docker
Para configurar un contenedor de Docker que ejecute Redis, utilizamos el siguiente comando:
```sh
docker run -d --name redis-stack -e REDIS_ARGS="--requirepass <YourHardPasswordHere>" -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
```

- -d: Ejecuta el contenedor en segundo plano.
- --name redis-stack: Asigna el nombre redis-stack al contenedor.
- -e REDIS_ARGS="--requirepass <YourHardPasswordHere>": Establece una contraseña para Redis.
- -p 6379:6379: Mapea el puerto 6379 del contenedor al puerto 6379 de la máquina host.
- -p 8001:8001: Mapea el puerto 8001 del contenedor al puerto 8001 de la máquina host.
- redis/redis-stack:latest: Especifica la imagen más reciente de Redis.

Una vez que el contenedor de Redis esté en funcionamiento, puede acceder a Redis Browser en la siguiente URL:
>Open Redis Browser http://localhost:8001/

>Username: default

>Password: You have set it up
