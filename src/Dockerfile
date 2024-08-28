# Utiliser l'image officielle .NET en version 8.0 pour la construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Copier les fichiers du projet dans le conteneur
COPY . ./

# Restaurer les dépendances
RUN dotnet restore

# Publier l'application
RUN dotnet publish -c Release -o out

# Utiliser l'image runtime .NET en version 8.0 pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build-env /out .

# Définir le point d'entrée de l'application
ENTRYPOINT ["dotnet", "LionkApp.dll"]
