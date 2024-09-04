# Use the official .NET SDK image for the build process
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project file
COPY . ./

# Restore dependencies
RUN dotnet restore App/LionkApp/LionkApp.csproj

# Publish the application as a single executable file
RUN dotnet publish App/LionkApp/LionkApp.csproj -c Release -p:PublishSingleFile=true --self-contained -o /app/out

# Use the official .NET runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app

# Set the user to run the application
USER root

# Copy the published application to the runtime image
COPY --from=build-env /app/out .

# Set the permissions to execute the application
RUN chmod +x LionkApp

# Set the entry point
ENTRYPOINT ["./LionkApp"]
