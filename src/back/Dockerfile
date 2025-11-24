FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln .
COPY ["Mariage.Api/Mariage.Api.csproj", "Mariage.Api/"]
COPY ["Mariage.Application/Mariage.Application.csproj", "Mariage.Application/"]
COPY ["Mariage.Contracts/Mariage.Contracts.csproj", "Mariage.Contracts/"]
COPY ["Mariage.Domain/Mariage.Domain.csproj", "Mariage.Domain/"]
COPY ["Mariage.Infrastructure/Mariage.Infrastructure.csproj", "Mariage.Infrastructure/"]
RUN dotnet restore "Mariage.Api/Mariage.Api.csproj"
COPY . .
WORKDIR "/src/Mariage.Api"
RUN dotnet build "Mariage.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mariage.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mariage.Api.dll"]
