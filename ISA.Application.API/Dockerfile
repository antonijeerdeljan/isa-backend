#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
EXPOSE 7109
EXPOSE 5121

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ISA.Application.API/ISA.Application.API.csproj", "ISA.Application.API/"]
COPY ["ISA.Core.Domain/ISA.Core.Domain.csproj", "ISA.Core.Domain/"]
COPY ["ISA.Core.Infrastructure.HttpClient/ISA.Core.Infrastructure.HttpClient.csproj", "ISA.Core.Infrastructure.HttpClient/"]
COPY ["ISA.Core.Infrastructure.Identity/ISA.Core.Infrastructure.Identity.csproj", "ISA.Core.Infrastructure.Identity/"]
COPY ["ISA.Core.Infrastructure.Persistence.PostgreSQL/ISA.Core.Infrastructure.Persistence.PostgreSQL.csproj", "ISA.Core.Infrastructure.Persistence.PostgreSQL/"]
RUN dotnet restore "ISA.Application.API/ISA.Application.API.csproj"
COPY . .
WORKDIR "/src/ISA.Application.API"
RUN dotnet build "ISA.Application.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ISA.Application.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ISA.Application.API.dll"]