FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DeliveryService.Api/DeliveryService.Api.csproj", "DeliveryService.Api/"]
COPY ["DeliveryService.Infra.IoC/DeliveryService.Infra.IoC.csproj", "DeliveryService.Infra.IoC/"]
COPY ["DeliveryService.Infra.Data/DeliveryService.Infra.Data.csproj", "DeliveryService.Infra.Data/"]
COPY ["DeliveryService.Domain/DeliveryService.Domain.csproj", "DeliveryService.Domain/"]
COPY ["DeliveryService.Infra/DeliveryService.Infra.csproj", "DeliveryService.Infra/"]
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