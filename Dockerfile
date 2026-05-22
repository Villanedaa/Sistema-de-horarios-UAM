FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SistemaHorarios.API/SistemaHorarios.API.csproj", "SistemaHorarios.API/"]
COPY ["SistemaHorarios.Logica/SistemaHorarios.Logica.csproj", "SistemaHorarios.Logica/"]
COPY ["SistemaHorarios.Datos/SistemaHorarios.Datos.csproj", "SistemaHorarios.Datos/"]
COPY ["SistemaHorarios.Modelos/SistemaHorarios.Modelos.csproj", "SistemaHorarios.Modelos/"]

RUN dotnet restore "SistemaHorarios.API/SistemaHorarios.API.csproj"

COPY . .

RUN dotnet publish "SistemaHorarios.API/SistemaHorarios.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SistemaHorarios.API.dll"]
