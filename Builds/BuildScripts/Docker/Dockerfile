# docker build -f Dockerfile -t rico/myapi .
# Build runtime image 
FROM microsoft/aspnetcore
MAINTAINER RiCo
WORKDIR /app

COPY ./AssemblyOutput /app
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT  ["dotnet", "MyAPI.dll"]

