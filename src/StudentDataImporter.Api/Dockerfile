FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
COPY ["src/StudentDataImporter.Api/StudentDataImporter.Api.csproj", "./StudentDataImporter.Api/"]
RUN dotnet restore "./StudentDataImporter.Api/StudentDataImporter.Api.csproj"
COPY . .
WORKDIR "/app/src/StudentDataImporter.Api"
RUN dotnet restore

RUN dotnet build "./StudentDataImporter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./StudentDataImporter.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentDataImporter.Api.dll"]