FROM alpine:3.18.0 AS builder
RUN apk add --no-cache dotnet7-sdk
COPY . /tmp/builder
RUN ls -lahFi "/tmp/builder"
RUN dotnet publish "/tmp/builder" --configuration Release --no-self-contained --output "/tmp/builder/publish"

FROM alpine:3.18.0
RUN apk add --no-cache aspnetcore7-runtime
RUN adduser -D dotnet dotnet
COPY --from=builder --chown=dotnet:dotnet /tmp/builder/publish /home/dotnet/app
USER dotnet
CMD [ dotnet, /home/dotnet/app/MartinSimulator.dll ]
