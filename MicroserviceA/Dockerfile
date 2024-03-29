#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MicroserviceA/MicroserviceA.csproj", "MicroserviceA/"]
COPY ["MicroserviceA.Messaging.Send/MicroserviceA.Messaging.Send.csproj", "MicroserviceA.Messaging.Send/"]
COPY ["MicroserviceA.Domain/MicroserviceA.Domain.csproj", "MicroserviceA.Domain/"]
COPY ["MicroserviceA.Service/MicroserviceA.Service.csproj", "MicroserviceA.Service/"]
RUN dotnet restore "MicroserviceA/MicroserviceA.csproj"
COPY . .
WORKDIR "/src/MicroserviceA"
RUN dotnet build "MicroserviceA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceA.dll"]