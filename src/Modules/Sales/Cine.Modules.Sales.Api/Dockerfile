FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8084

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Modules/Sales/Cine.Modules.Sales.Api/Cine.Modules.Sales.Api.csproj", "src/Modules/Sales/Cine.Modules.Sales.Api/"]
RUN dotnet restore "src/Modules/Sales/Cine.Modules.Sales.Api/Cine.Modules.Sales.Api.csproj"
COPY . .
WORKDIR "/src/src/Modules/Sales/Cine.Modules.Sales.Api"
RUN dotnet build "Cine.Modules.Sales.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cine.Modules.Sales.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cine.Modules.Sales.Api.dll"]
