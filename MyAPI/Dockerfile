#FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM microsoft/dotnet:2.1-sdk AS build
#WORKDIR /src
#COPY MyAPI/MyAPI.csproj MyAPI/
#RUN dotnet restore MyAPI/MyAPI.csproj
#COPY . .
#WORKDIR /src/MyAPI
#RUN dotnet build MyAPI.csproj -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish MyAPI.csproj -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "MyAPI.dll"]

#
# build
#
# # docker build -t consul:latest .
# docker build --rm -t consul:base .
# docker run -d -p 8500:8500 --name=dev-consul consul:base
# bootstrap server
#
# # docker run -d consul -server -bootstrap
# # docker inspect (container_id)
#
# servers
# # docker run -d --name=dev-consul -p 8500:8500 consul agent -ui -server -client=0.0.0.0 -bind=0.0.0.0
# # docker run -d consul -server -join=(server address)
# # docker run -d consul -server -join=(server address)
# docker run -d --net=host -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true}' consul agent -server -bind=<external ip> -retry-join=<root agent ip> -bootstrap-expect=<number of server agents>
#
# clients
#
# # docker run -d consul -join=(server address)
# # docker run -d consul -join=(server address)
# # docker run -d consul -join=(server address)
# sudo ./consul agent -ui -bootstrap -disable-host-node-id -node localnode -bind 127.0.0.1 -server -config-file=server.json 

FROM ubuntu

RUN apt-get update
RUN apt-get install -y curl unzip

RUN curl -O -L 'https://releases.hashicorp.com/consul/1.4.4/consul_1.4.4_linux_amd64.zip' && \
  unzip ./consul_1.4.4_linux_amd64.zip && \
  mv ./consul /usr/bin/ && \
  mkdir -p /opt/consul/data && \
  mkdir -p /opt/consul/etc && \
  rm consul_1.4.4_linux_amd64.zip

EXPOSE 8300
EXPOSE 8301 8301/udp 8302 8302/udp
EXPOSE 8500 8600 8600/udp
VOLUME /opt/consul

#COPY docker-entrypoint.sh /usr/local/bin/docker-entrypoint.sh
#ENTRYPOINT ["docker-entrypoint.sh"]

#CMD ["agent", "-dev", "-client", "0.0.0.0"]
#CMD ["/usr/bin/consul", "agent", "-data-dir=/opt/consul/data", "-config-dir=/opt/consul/etc", "-ui-dir=/opt/consul/ui", "-server", "-bootstrap"]

#CMD ["-server", "-bootstrap"]
ENTRYPOINT ["/usr/bin/consul", "agent","-ui","-bootstrap", "-disable-host-node-id","-node","localnode","-server","-client","0.0.0.0","-bind","0.0.0.0","-data-dir=/opt/consul/data", "-config-dir=/opt/consul/etc"]
