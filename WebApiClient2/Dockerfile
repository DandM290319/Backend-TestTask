FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY WebApiClient2/*.csproj ./WebApiClient2/
RUN dotnet restore ./WebApiClient2/WebApiClient2.csproj

COPY WebApiClient2/. ./WebApiClient2/
RUN dotnet publish ./WebApiClient2/WebApiClient2.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebApiClient2.dll"]