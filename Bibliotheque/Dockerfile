FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY Bibliotheque/Bibliotheque.csproj ./Bibliotheque/
RUN dotnet restore ./Bibliotheque/Bibliotheque.csproj

COPY . ./
RUN dotnet publish ./Bibliotheque/Bibliotheque.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Bibliotheque.dll"]