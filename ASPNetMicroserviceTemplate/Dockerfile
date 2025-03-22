# https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile

# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore dependencies
COPY ["ASPNetMicroserviceTemplate/ASPNetMicroserviceTemplate.csproj", "ASPNetMicroserviceTemplate/"]
RUN dotnet restore "ASPNetMicroserviceTemplate/ASPNetMicroserviceTemplate.csproj"

# Build the app
COPY . .
WORKDIR "/src/ASPNetMicroserviceTemplate"
RUN dotnet build "ASPNetMicroserviceTemplate.csproj" -c Release -o /app/build

# Stage 2: Publish the app
FROM build AS publish
RUN dotnet publish "ASPNetMicroserviceTemplate.csproj" -c Release -o /app/publish

# Stage 3: Run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=8081
EXPOSE 8081
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASPNetMicroserviceTemplate.dll"]