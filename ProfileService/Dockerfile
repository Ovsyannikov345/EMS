FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ProfileService.sln ./

COPY ProfileService/ProfileService.csproj ProfileService/
COPY ProfileService.BLL/ProfileService.BLL.csproj ProfileService.BLL/
COPY ProfileService.DAL/ProfileService.DAL.csproj ProfileService.DAL/

RUN dotnet restore ProfileService/ProfileService.csproj

COPY . .

WORKDIR /src/ProfileService
RUN dotnet build --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProfileService.dll"]