FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ChatService.sln ./

COPY ChatService/ChatService.csproj ChatService/
COPY ChatService.BLL/ChatService.BLL.csproj ChatService.BLL/
COPY ChatService.DAL/ChatService.DAL.csproj ChatService.DAL/
COPY MessageBus/MessageBus.csproj MessageBus/

RUN dotnet restore ChatService/ChatService.csproj

COPY . .

WORKDIR /src/ChatService
RUN dotnet build --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatService.dll"]