#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/DockerBootstrap/DockerBootstrap.csproj", "src/DockerBootstrap/"]
COPY ["src/thinkhomeLogger.NLog/ThinkhomeLogger.NLog.csproj", "src/thinkhomeLogger.NLog/"]
RUN dotnet restore "src/DockerBootstrap/DockerBootstrap.csproj"
COPY . .
WORKDIR "/src/src/DockerBootstrap"
RUN dotnet build "DockerBootstrap.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerBootstrap.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerBootstrap.dll"]