FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/TokenService.STS.Identity/TokenService.STS.Identity.csproj", "Services/TokenService.STS.Identity/"]
COPY ["Services/TokenService.Admin.EntityFramework.Shared/TokenService.Admin.EntityFramework.Shared.csproj", "Services/TokenService.Admin.EntityFramework.Shared/"]
COPY ["Services/TokenService.Shared/TokenService.Shared.csproj", "Services/TokenService.Shared/"]
RUN dotnet restore "Services/TokenService.STS.Identity/TokenService.STS.Identity.csproj"
COPY . .
WORKDIR "/src/Services/TokenService.STS.Identity"
RUN dotnet build "TokenService.STS.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TokenService.STS.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "TokenService.STS.Identity.dll"]