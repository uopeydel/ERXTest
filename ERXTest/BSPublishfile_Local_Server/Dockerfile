# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

FROM mcr.microsoft.com/dotnet/aspnet:5.0.2-focal-amd64 AS runtime
WORKDIR /app
COPY * /app/
RUN ls
RUN pwd
ENTRYPOINT ["dotnet", "ERXTest.Server.dll","--environment=UAT"]