FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
COPY ["src/StudentApi/StudentApi.csproj", "./StudentApi/"]
RUN dotnet restore "./StudentApi/StudentApi.csproj"
COPY . .
WORKDIR "/app/src/StudentApi"
RUN dotnet restore

RUN dotnet build "./StudentApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./StudentApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentApi.dll"]