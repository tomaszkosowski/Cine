FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Modules/Shows/Cine.Modules.Shows.Api/Cine.Modules.Shows.Api.csproj", "src/Modules/Shows/Cine.Modules.Shows.Api/"]
RUN dotnet restore "src/Modules/Shows/Cine.Modules.Shows.Api/Cine.Modules.Shows.Api.csproj"
COPY . .
WORKDIR "/src/src/Modules/Shows/Cine.Modules.Shows.Api"
RUN dotnet build "Cine.Modules.Shows.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cine.Modules.Shows.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cine.Modules.Shows.Api.dll"]
