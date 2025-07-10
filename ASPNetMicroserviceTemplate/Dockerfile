# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files and build the app
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the official .NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update -y && apt-get upgrade -y && apt-get install -y \ 
    curl 
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /out .

# Expose the port the app runs on
ENV ASPNETCORE_HTTP_PORTS=8081
EXPOSE 8081

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ASPNetMicroserviceTemplate.dll"]

# Строка сборки образа
# docker build . --platform linux/amd64 -t vegiwoo/aspnetmicroservicetemplate:1.0.1