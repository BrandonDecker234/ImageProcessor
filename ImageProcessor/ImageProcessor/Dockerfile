FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 9000
EXPOSE 9001

# Fix SSL certificate issues in Docker (run as root first)
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

# Set environment variables for SSL fixes
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=""
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=""

# Switch to non-root user after installing packages
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImageProcessor/ImageProcessor.csproj", "ImageProcessor/"]
RUN dotnet restore "ImageProcessor/ImageProcessor.csproj"
COPY . .
WORKDIR "/src/ImageProcessor"
RUN dotnet build "ImageProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ImageProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageProcessor.dll"]
