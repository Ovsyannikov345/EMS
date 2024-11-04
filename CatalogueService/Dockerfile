FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CatalogueService.sln ./

COPY CatalogueService/CatalogueService.csproj CatalogueService/
COPY CatalogueService.BLL/CatalogueService.BLL.csproj CatalogueService.BLL/
COPY CatalogueService.DAL/CatalogueService.DAL.csproj CatalogueService.DAL/
COPY MessageBus/MessageBus.csproj MessageBus/

RUN dotnet restore CatalogueService/CatalogueService.csproj

COPY . .

WORKDIR /src/CatalogueService
RUN dotnet build --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogueService.dll"]