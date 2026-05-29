# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copier les fichiers et restaurer les dépendances
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Étape finale (exécution)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Bibliotheque.dll"]