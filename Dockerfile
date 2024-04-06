FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# --------------

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /work

# Copy all files to see if any .csproj files exist
COPY . .

# Copy .csproj files if they exist
COPY ResumeCreator/*.csproj ./

# Check if .csproj files exist before running the RUN command
RUN if ls *.csproj 1> /dev/null 2>&1; then \
    for projectFile in *.csproj; do \
      mkdir -p ${projectFile%.*}/ && mv $projectFile ${projectFile%.*}/; \
    done; \
  fi

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
 
# Specify the exact path to the project file you want to publish
RUN dotnet restore /work/ResumeCreator/CVCreator.csproj

COPY ResumeCreator .

# --------------

FROM build AS publish
WORKDIR /work

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# Specify the exact path to the project file you want to publish
RUN dotnet publish -c Release -o /app --no-restore /work/ResumeCreator/CVCreator.csproj

# --------------

FROM base AS final
COPY --from=publish /app .

# Switch to root user to perform permission changes
USER root

# Set permissions for each file to be readable and writable
RUN chmod -R 777 /app/*

# Set permissions for each directory to be readable, writable, and executable
RUN find /app -type d -exec chmod 777 {} +


# Switch back to non-root user
USER $APP_UID

ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

HEALTHCHECK --interval=5m --timeout=3s --start-period=10s --retries=1 \
  CMD curl --fail http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CVCreator.dll"]
