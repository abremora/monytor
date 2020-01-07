FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["Monytor.Scheduler.NetCore/Monytor.Scheduler.NetCore.csproj","Monytor.Scheduler.NetCore/"]
COPY ["Monytor.RavenDb/Monytor.RavenDb.csproj","Monytor.RavenDb/"]
COPY ["Monytor.Core/Monytor.Core.csproj","Monytor.Core/"]
COPY ["Monytor.Startup/Monytor.Startup.csproj","Monytor.Startup/"]
COPY ["Monytor.Infrastructure/Monytor.Infrastructure.csproj","Monytor.Infrastructure/"]
COPY ["Monytor.PostgreSQL/Monytor.PostgreSQL.csproj","Monytor.PostgreSQL/"]
COPY ["Monytor.Implementation/Monytor.Implementation.csproj","Monytor.Implementation/"]
COPY ["Monytor.Scheduler/Monytor.Scheduler.csproj","Monytor.Scheduler/"]
COPY ["Monytor.Implementation.Collectors.Sql.MsSql/Monytor.Implementation.Collectors.Sql.MsSql.csproj","Monytor.Implementation.Collectors.Sql.MsSql/"]
COPY ["Monytor.Implementation.Collectors.Sql/Monytor.Implementation.Collectors.Sql.csproj"," Monytor.Implementation.Collectors.Sql/"]
COPY ["Monytor.Implementation.Collectors.Sql.PostgreSql/Monytor.Implementation.Collectors.Sql.PostgreSql.csproj"," Monytor.Implementation.Collectors.Sql.PostgreSql/"]
COPY ["Monytor.Implementation.Collectors/Monytor.Implementation.Collectors.csproj"," Monytor.Implementation.Collectors/"]
COPY ["Monytor.Implementation.Collectors.RavenDb/Monytor.Implementation.Collectors.RavenDb.csproj"," Monytor.Implementation.Collectors.RavenDb/"]
COPY ["Monytor.Implementation.Collectors.Sql.MySql/Monytor.Implementation.Collectors.Sql.MySql.csproj"," Monytor.Implementation.Collectors.Sql.MySql/"]
COPY ["Monytor.Implementation.Collectors.Sql.Oracle/Monytor.Implementation.Collectors.Sql.Oracle.csproj"," Monytor.Implementation.Collectors.Sql.Oracle/"]
RUN dotnet restore "Monytor.Scheduler.NetCore/Monytor.Scheduler.NetCore.csproj"
COPY . .
WORKDIR "/src/Monytor.Scheduler.NetCore"
RUN dotnet build "Monytor.Scheduler.NetCore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monytor.Scheduler.NetCore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "Monytor.Scheduler.NetCore.dll"]
