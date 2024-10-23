FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY WebApiClient3/*.csproj ./WebApiClient3/
RUN dotnet restore ./WebApiClient3/WebApiClient3.csproj

COPY WebApiClient3/. ./WebApiClient3/
RUN dotnet publish ./WebApiClient3/WebApiClient3.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebApiClient3.dll"]