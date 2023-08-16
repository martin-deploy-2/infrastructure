FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
COPY . /tmp/builder
RUN dotnet publish "/tmp/builder" --configuration Release --no-self-contained --output "/tmp/builder/publish"

FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY --from=builder /tmp/builder/publish /home/dotnet/app
ENTRYPOINT ["dotnet", "/home/dotnet/app/MartinSimulator.dll"]
