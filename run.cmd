docker stop spica
docker rm spica
docker run -it --rm --name spica -p 8080:80 -p 8081:443 --env-file=dockerenv -v %APPDATA%/Microsoft/UserSecrets/:/root/.microsoft/usersecrets:ro -v %USERPROFILE%/.aspnet/https/:/root/.aspnet/https -v %CD%\svelte-app/:/app/svelte-app/ spica