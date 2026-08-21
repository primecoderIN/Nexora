FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and projects for restore
COPY ["Nexora.slnx", "./"]
COPY ["Nexora.Api/Nexora.Api.csproj", "Nexora.Api/"]
COPY ["Nexora.Shared/Nexora.Shared.csproj", "Nexora.Shared/"]
COPY ["Nexora.Modules/Identity/Nexora.Modules.Identity.csproj", "Nexora.Modules/Identity/"]

# Restore dependencies
RUN dotnet restore "Nexora.slnx"

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR "/src/Nexora.Api"
RUN dotnet publish "Nexora.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Run the API
ENTRYPOINT ["dotnet", "Nexora.Api.dll"]
