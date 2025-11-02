# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY GeminiOrderService.sln ./
COPY src/GeminiOrderService.Api/GeminiOrderService.Api.csproj ./src/GeminiOrderService.Api/
COPY src/GeminiOrderService.Application/GeminiOrderService.Application.csproj ./src/GeminiOrderService.Application/
COPY src/GeminiOrderService.Contracts/GeminiOrderService.Contracts.csproj ./src/GeminiOrderService.Contracts/
COPY src/GeminiOrderService.Domain/GeminiOrderService.Domain.csproj ./src/GeminiOrderService.Domain/
COPY src/GeminiOrderService.Infrastructure/GeminiOrderService.Infrastructure.csproj ./src/GeminiOrderService.Infrastructure/

# Restore NuGet packages
RUN dotnet restore GeminiOrderService.sln

# Copy the entire source code
COPY . ./

# Build and publish the API project
RUN dotnet publish src/GeminiOrderService.Api/GeminiOrderService.Api.csproj -c Release -o out

# Use the official .NET 9.0 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build-env /app/out .

# Install essential debugging tools as root
RUN apt-get update && \
    apt-get install -y \
    curl \
    iputils-ping \
    telnet \
    dnsutils \
    net-tools \
    wget \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


# Set the entry point for the container
ENTRYPOINT ["dotnet", "GeminiOrderService.Api.dll"]