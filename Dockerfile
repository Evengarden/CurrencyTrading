#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./CurrencyTrading.src/CurrencyTrading.csproj", "CurrencyTrading.src/"]
COPY ["./CurrencyTrading.DAL/CurrencyTrading.DAL.csproj", "CurrencyTrading.DAL/"]
COPY ["./CurrencyTrading.services/CurrencyTrading.services.csproj", "CurrencyTrading.services/"]
RUN dotnet restore "CurrencyTrading.src/CurrencyTrading.csproj"
COPY . .
WORKDIR "/src/CurrencyTrading.src"
RUN dotnet build "CurrencyTrading.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyTrading.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyTrading.dll"]