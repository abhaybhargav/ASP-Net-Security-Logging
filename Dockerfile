# Use the official Microsoft .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["SecurityLoggingDemo.csproj", "./"]
RUN dotnet restore

# Copy the rest of the source code and build the project
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port 8880
EXPOSE 8880

# Set the entry point for the application
ENTRYPOINT ["dotnet", "SecurityLoggingDemo.dll"]