FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["delegaga/delegaga.csproj", "delegaga/"]
RUN dotnet restore "delegaga/delegaga.csproj"
COPY . .
WORKDIR "/src/delegaga"
RUN dotnet build "delegaga.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "delegaga.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "delegaga.dll"]
