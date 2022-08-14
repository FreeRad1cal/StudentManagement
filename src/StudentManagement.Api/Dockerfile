FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
COPY ["src/StudentManagement.Api/StudentManagement.Api.csproj", "./StudentManagement.Api/"]
RUN dotnet restore "./StudentManagement.Api/StudentManagement.Api.csproj"
COPY . .
WORKDIR "/app/src/StudentManagement.Api"
RUN dotnet restore

RUN dotnet build "./StudentManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./StudentManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManagement.Api.dll"]