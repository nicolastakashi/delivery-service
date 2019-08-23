FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DeliveryService.Api/DeliveryService.Api.csproj", "DeliveryService.Api/"]
RUN dotnet restore "DeliveryService.Api/DeliveryService.Api.csproj"
COPY . .
WORKDIR "/src/DeliveryService.Api"
RUN dotnet build "DeliveryService.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DeliveryService.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DeliveryService.Api.dll"]