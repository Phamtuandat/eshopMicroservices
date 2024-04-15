#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Ordering/Order.API/Ordering.API.csproj", "src/Services/Ordering/Order.API/"]
COPY ["src/Services/Ordering/Order.Application/Ordering.Application.csproj", "src/Services/Ordering/Order.Application/"]
COPY ["src/BuildingBlocks/BuildingBlocks.csproj", "src/BuildingBlocks/"]
COPY ["src/Services/Ordering/Order.Domain/Ordering.Domain.csproj", "src/Services/Ordering/Order.Domain/"]
COPY ["src/Services/Ordering/Order.Infrastructure/Ordering.Infrastructure.csproj", "src/Services/Ordering/Order.Infrastructure/"]
RUN dotnet restore "./src/Services/Ordering/Order.API/./Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/Order.API"
RUN dotnet build "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]