FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY WebApiClient1/*.csproj ./WebApiClient1/
RUN dotnet restore ./WebApiClient1/WebApiClient1.csproj

COPY WebApiClient1/. ./WebApiClient1/
RUN dotnet publish ./WebApiClient1/WebApiClient1.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebApiClient1.dll"]