FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app
 
#copia la aplicacion a dir app
COPY *.sln .

COPY ToDoApp.API/*.csproj ./ToDoApp.API/
COPY ToDoApp.Application/*.csproj ./ToDoApp.Application/
COPY ToDoApp.Application.Tests/*.csproj ./ToDoApp.Application.Tests/
COPY ToDoApp.Domain/*.csproj ./ToDoApp.Domain/
COPY ToDoApp.Infrastructure/*.csproj ./ToDoApp.Infrastructure/
COPY ToDoApp.Persistence/*.csproj ./ToDoApp.Persistence/

#restaura las dependencias
RUN dotnet restore

COPY . .

RUN dotnet publish ToDoApp.API/ToDoApp.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet","ToDoApp.API.dll" ]

EXPOSE 5000