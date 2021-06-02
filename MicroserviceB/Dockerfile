#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MicroserviceB/MicroserviceB.csproj", "MicroserviceB/"]
COPY ["MicroserviceB.Messaging.Receive/MicroserviceB.Messaging.Receive.csproj", "MicroserviceB.Messaging.Receive/"]
COPY ["MicroserviceB.Service/MicroserviceB.Service.csproj", "MicroserviceB.Service/"]
RUN dotnet restore "MicroserviceB/MicroserviceB.csproj"
COPY . .
WORKDIR "/src/MicroserviceB"
RUN dotnet build "MicroserviceB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceB.dll"]
