FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Modules/Theater/Cine.Modules.Theater.Api/Cine.Modules.Theater.Api.csproj", "src/Modules/Theater/Cine.Modules.Theater.Api/"]
RUN dotnet restore "src/Modules/Theater/Cine.Modules.Theater.Api/Cine.Modules.Theater.Api.csproj"
COPY . .
WORKDIR "/src/src/Modules/Theater/Cine.Modules.Theater.Api"
RUN dotnet build "Cine.Modules.Theater.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cine.Modules.Theater.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cine.Modules.Theater.Api.dll"]
