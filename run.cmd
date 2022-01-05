docker stop spica
docker rm spica
docker run -it --rm --name spica -p 8080:80 -p 8081:443 -p 33440:33440 -p 35729:35729 -p 5000:5000 --env-file=dockerenv -v %APPDATA%/Microsoft/UserSecrets/:/root/.microsoft/usersecrets:ro -v %USERPROFILE%/.aspnet/https/:/root/.aspnet/https -v %CD%\svelte-app/src:/app/svelte-app/ spica