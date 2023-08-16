FROM alpine:3.18.0 AS builder
RUN apk add --no-cache dotnet7-sdk
COPY . /tmp/builder
RUN dotnet publish "/tmp/builder" --configuration Release --no-self-contained --output "/tmp/builder/publish"

FROM alpine:3.18.0
RUN apk add --no-cache aspnetcore7-runtime
COPY --from=builder --chown=dotnet:dotnet /tmp/builder/publish /home/dotnet/app
EXPOSE 5000/tcp
ENTRYPOINT [ "ash", "-c", "dotnet /home/dotnet/app/MartinSimulator.dll" ]
