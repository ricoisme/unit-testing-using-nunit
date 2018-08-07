docker search consul
docker pull consul

docker run -d -p 8400:8400 -p 8500:8500 -p 5200:5200 -p 8600:53/udp --name=dev-consul -e CONSUL_BIND_INTERFACE=eth0 consul

docker run -d -p 8400:8400 -p 8500:8500 -p 8600:53/udp --name=dev-consul -e CONSUL_BIND_INTERFACE=eth0 consul


--start consul server
docker run -d -p 8400:8400 -p 8500:8500 -p 8600:53/udp --name consul-server-node1 -bootstrap -advertise 127.0.0.1

docker run \
   -d \
   -e CONSUL_LOCAL_CONFIG='{
    "datacenter":"us_west",
    "server":true,
    "enable_debug":true
    }' \
   consul agent -server -bootstrap-expect=3
   
docker run -d -p 8500:8500 -p 5200:5200 --name=node1 -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true}' consul agent -server -bind=127.0.0.1
docker run -d --net=host -e 'CONSUL_LOCAL_CONFIG={"leave_on_terminate": true}' consul agent -bind=<external ip> -retry-join=<root agent ip>
 
docker inspect dev-consul | grep "IPAddress"


docker run -d -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true}' --name=node1 consul agent -server -bind=127.0.0.1 -retry-join=<root agent ip> -bootstrap-expect=<number of server agents>

docker run -d -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true}' --name=node1 consul agent -server -bind=127.0.0.1 -bootstrap-expect=3 -node=node1

-node：节点的名称
-bind：绑定的一个地址，用于节点之间通信的地址，可以是内外网，必须是可以访问到的地址
-server：这个就是表示这个节点是个SERVER
-bootstrap-expect：这个就是表示期望提供的SERVER节点数目，数目一达到，它就会被激活，然后就是LEADER了

http://localhost:8500/ui/dc1/services

https://www.jianshu.com/p/f8746b81d65d

https://hub.docker.com/_/consul/



