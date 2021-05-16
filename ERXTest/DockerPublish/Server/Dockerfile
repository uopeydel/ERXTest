# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

FROM mcr.microsoft.com/dotnet/aspnet:5.0.2-focal-amd64 AS runtime
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY PublishFile/* /app/
RUN mkdir wwwroot
RUN cp -avr xls_template/ wwwroot/
RUN ls
RUN pwd
ENTRYPOINT ["dotnet", "ERXTest.Server.dll","--environment=UAT"]