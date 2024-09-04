FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

COPY ["src/CleanArchitecture.Course.Project.Domain/CleanArchitecture.Course.Project.Domain.csproj", "src/CleanArchitecture.Course.Project.Domain/"]
COPY ["src/CleanArchitecture.Course.Project.Application/CleanArchitecture.Course.Project.Application.csproj", "src/CleanArchitecture.Course.Project.Application/"]
COPY ["src/CleanArchitecture.Course.Project.Infrastructure/CleanArchitecture.Course.Project.Infrastructure.csproj", "src/CleanArchitecture.Course.Project.Infrastructure/"]
COPY ["src/CleanArchitecture.Course.Project.Api/CleanArchitecture.Course.Project.Api.csproj", "src/CleanArchitecture.Course.Project.Api/"]

RUN dotnet restore "src/CleanArchitecture.Course.Project.Api/CleanArchitecture.Course.Project.Api.csproj"
COPY . .
WORKDIR "/src/src/CleanArchitecture.Course.Project.Api"
RUN dotnet build "CleanArchitecture.Course.Project.Api.csproj" -c Debug -o /app/build

FROM build AS publish

RUN dotnet publish "CleanArchitecture.Course.Project.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Course.Project.Api.dll"]
