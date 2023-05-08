FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/TokenService.Admin/TokenService.Admin.csproj", "Services/TokenService.Admin/"]
COPY ["Services/TokenService.Admin.EntityFramework.Shared/TokenService.Admin.EntityFramework.Shared.csproj", "Services/TokenService.Admin.EntityFramework.Shared/"]
COPY ["Services/TokenService.Admin.EntityFramework.SqlServer/TokenService.Admin.EntityFramework.SqlServer.csproj", "Services/TokenService.Admin.EntityFramework.SqlServer/"]
COPY ["Services/TokenService.Shared/TokenService.Shared.csproj", "Services/TokenService.Shared/"]
COPY ["Services/TokenService.Admin.EntityFramework.PostgreSQL/TokenService.Admin.EntityFramework.PostgreSQL.csproj", "Services/TokenService.Admin.EntityFramework.PostgreSQL/"]
COPY ["Services/TokenService.Admin.EntityFramework.MySql/TokenService.Admin.EntityFramework.MySql.csproj", "Services/TokenService.Admin.EntityFramework.MySql/"]
RUN dotnet restore "Services/TokenService.Admin/TokenService.Admin.csproj"
COPY . .
WORKDIR "/src/Services/TokenService.Admin"
RUN dotnet build "TokenService.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TokenService.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "TokenService.Admin.dll"]