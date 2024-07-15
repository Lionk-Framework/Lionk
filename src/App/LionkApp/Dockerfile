# Utiliser l'image officielle .NET en version 7.0 pour la construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copier les fichiers .csproj et restaurer les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copier le reste des fichiers du projet et publier l'application
COPY . ./
RUN dotnet publish -c Release -o out

# Utiliser l'image runtime .NET en version 7.0 pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Exposer le port utilisé par l'application
EXPOSE 5000

# Définir le point d'entrée de l'application
ENTRYPOINT ["dotnet", "LionkApp.dll"]
