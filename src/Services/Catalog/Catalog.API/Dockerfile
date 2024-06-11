#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PASSWORD_ENV_SEEDED=some.long.password.fllkwefiwejf23049uwlekjf.sEFWEFGR98^&$
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p $PASSWORD_ENV_SEEDED
WORKDIR /etc
RUN apt-get update
RUN apt-get install curl -y
COPY ["nuget-org.pem", "ssl/certs/nuget-org.pem"]
ENV SSL_CERT_FILE=/etc/ssl/certs/nuget-org.pem
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
RUN dotnet restore "./Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --chmod=0755 --from=build /https/* /https/
WORKDIR /etc
RUN apt-get update
RUN apt-get install curl -y
COPY ["nuget-org.pem", "ssl/certs/nuget-org.pem"]
ENV SSL_CERT_FILE=/etc/ssl/certs/nuget-org.pem
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]