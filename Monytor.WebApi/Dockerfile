FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY Monytor.WebApi/package.json Monytor.WebApi/package.json 
RUN apk add --update npm && npm install -g gulp@4.0.2 && npm install gulp@4.0.2
RUN cd Monytor.WebApi && npm install

COPY ["Monytor.WebApi/Monytor.WebApi.csproj", "Monytor.WebApi/"]
COPY ["Monytor.Domain/Monytor.Domain.csproj", "Monytor.Domain/"]
COPY ["Monytor.Implementation/Monytor.Implementation.csproj", "Monytor.Implementation/"]
COPY ["Monytor.Core/Monytor.Core.csproj", "Monytor.Core/"]
COPY ["Monytor.Contracts/Monytor.Contracts.csproj", "Monytor.Contracts/"]
COPY ["Monytor.Infrastructure/Monytor.Infrastructure.csproj", "Monytor.Infrastructure/"]
COPY ["Monytor.Implementation.Collectors.Sql.MsSql/Monytor.Implementation.Collectors.Sql.MsSql.csproj", "Monytor.Implementation.Collectors.Sql.MsSql/"]
COPY ["Monytor.Implementation.Collectors.Sql/Monytor.Implementation.Collectors.Sql.csproj", "Monytor.Implementation.Collectors.Sql/"]
COPY ["Monytor.Implementation.Collectors.NetFramework/Monytor.Implementation.Collectors.NetFramework.csproj", "Monytor.Implementation.Collectors.NetFramework/"]
COPY ["Monytor.Implementation.Collectors.Sql.PostgreSql/Monytor.Implementation.Collectors.Sql.PostgreSql.csproj", "Monytor.Implementation.Collectors.Sql.PostgreSql/"]
COPY ["Monytor.Implementation.Collectors/Monytor.Implementation.Collectors.csproj", "Monytor.Implementation.Collectors/"]
COPY ["Monytor.Implementation.Collectors.RavenDb/Monytor.Implementation.Collectors.RavenDb.csproj", "Monytor.Implementation.Collectors.RavenDb/"]
COPY ["Monytor.RavenDb/Monytor.RavenDb.csproj", "Monytor.RavenDb/"]
COPY ["Monytor.Implementation.Collectors.Sql.MySql/Monytor.Implementation.Collectors.Sql.MySql.csproj", "Monytor.Implementation.Collectors.Sql.MySql/"]
COPY ["Monytor.Implementation.Collectors.Sql.Oracle/Monytor.Implementation.Collectors.Sql.Oracle.csproj", "Monytor.Implementation.Collectors.Sql.Oracle/"]
COPY ["Monytor.PostgreSQL/Monytor.PostgreSQL.csproj", "Monytor.PostgreSQL/"]
RUN dotnet restore "Monytor.WebApi/Monytor.WebApi.csproj"
COPY . .
WORKDIR "/src/Monytor.WebApi"
RUN gulp -f gulpfile.js && dotnet build "Monytor.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Monytor.WebApi.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Monytor.WebApi.dll"]