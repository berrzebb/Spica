dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\Spica.pfx -p banana
dotnet dev-certs https --trust
dotnet user-secrets -p Spica.csproj set "Kestrel:Certificates:Development:Password" "banana"
docker rmi spica
docker build . -t spica