FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ./ProjectManager/ProjectManager.csproj ./ProjectManager/ProjectManager.csproj

# restore only main project, it references everything that is required
RUN dotnet restore ./ProjectManager/ProjectManager.csproj

COPY ./ProjectManager ./ProjectManager

RUN dotnet publish ./ProjectManager/ProjectManager.csproj -c Release -o out --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out /app

ENTRYPOINT ["dotnet", "/app/ProjectManager.dll"]