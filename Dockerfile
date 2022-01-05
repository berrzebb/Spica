# Stage 1 Resotring & Compiling Backend
# Build Env Setting
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as backend
WORKDIR /build

# Copy and Restore as distinct layers
COPY *.csproj ./
RUN dotnet restore -r linux-musl-x64

COPY . .
RUN dotnet publish -c Release -o publish -r linux-musl-x64 --self-contained false --no-restore

# Stage 2 Create Image for compiled app
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
RUN apk add --update nodejs npm
COPY --from=backend /build/publish .
COPY svelte-app/package.json .
RUN npm install
COPY svelte-app/rollup.config.js .
COPY svelte-app/tsconfig.json .
COPY svelte-app/svelte.config.js .
COPY svelte-app/configuration.js .

ENTRYPOINT ["npm", "run", "dev"]