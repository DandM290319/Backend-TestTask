FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80

COPY ./WebApiServer/WebApiServer.csproj ./WebApiServer/
COPY ./WebApiClient1/WebApiClient1.csproj ./WebApiClient1/
COPY ./WebApiClient2/WebApiClient2.csproj ./WebApiClient2/
COPY ./WebApiClient3/WebApiClient3.csproj ./WebApiClient3/
RUN dotnet restore ./WebApiServer/WebApiServer.csproj

COPY . .
RUN dotnet publish ./WebApiServer/WebApiServer.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebApiServer.dll"]