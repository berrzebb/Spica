# Build Env Setting
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

# Copy and Restore as distinct layers
COPY *.csproj ./
RUN dotnet restore -r linux-musl-x64

COPY . ./
RUN dotnet publish -c Release -o out -r linux-musl-x64 --self-contained false --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Spica.dll"]