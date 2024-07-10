# Utiliser l'image de base officielle de Microsoft pour .NET 6.0 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Définir le répertoire de travail
WORKDIR /src

# Copier le fichier de solution et restaurer les dépendances
COPY Lionk.sln ./
COPY Lionk/Lionk.csproj ./Lionk/
RUN dotnet restore

# Copier tous les autres fichiers et construire l'application
COPY . ./
RUN dotnet publish -c Release -o /app

# Utiliser l'image de base officielle de Microsoft pour .NET 6.0 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Définir le répertoire de travail
WORKDIR /app

# Copier les fichiers publiés depuis l'étape de build
COPY --from=build /app .

# Exposer le port 5000
EXPOSE 5000

# Définir le point d'entrée pour l'application
ENTRYPOINT ["dotnet", "VotreProjet.dll"]