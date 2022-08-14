FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
COPY ["src/StudentDataImporterApi/StudentDataImporterApi.csproj", "./StudentDataImporterApi/"]
RUN dotnet restore "./StudentDataImporterApi/StudentDataImporterApi.csproj"
COPY . .
WORKDIR "/app/src/StudentDataImporterApi"
RUN dotnet restore

RUN dotnet build "./StudentDataImporterApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./StudentDataImporterApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentDataImporterApi.dll"]