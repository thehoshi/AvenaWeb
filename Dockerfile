FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AvenaCore/AvenaCore.csproj AvenaCore/
COPY AvenaPersistentSqlServer/AvenaPersistentSqlServer.csproj AvenaPersistentSqlServer/
COPY AvenaWeb/AvenaWeb.csproj AvenaWeb/

RUN dotnet restore AvenaWeb/AvenaWeb.csproj

COPY . .
RUN dotnet publish AvenaWeb/AvenaWeb.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "AvenaWeb.dll"]
