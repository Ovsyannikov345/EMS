FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NotificationService.sln ./

COPY NotificationService/NotificationService.csproj NotificationService/
COPY NotificationService.BLL/NotificationService.BLL.csproj NotificationService.BLL/
COPY NotificationService.DAL/NotificationService.DAL.csproj NotificationService.DAL/
COPY MessageBus/MessageBus.csproj MessageBus/

RUN dotnet restore NotificationService/NotificationService.csproj

COPY . .

WORKDIR /src/NotificationService
RUN dotnet build --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]