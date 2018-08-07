docker-compose -f docker-compose.yml up -d
docker exec -it container bash


./kafka-topics.sh --zookeeper kafka_zookeeper_1.kafka_default:2181 --list
./kafka-consumer-groups.sh --zookeeper 127.0.0.1:2181 --describe --group flume
./kafka-topics.sh --describe --zookeeper kafka_zookeeper_1.kafka_default:2181 --topic rico.services.customer.check

https://www.apache.org/dyn/closer.cgi?path=/kafka/2.0.0/kafka_2.11-2.0.0.tgz

http://www-eu.apache.org/dist/kafka/2.0.0/kafka_2.12-2.0.0.tgz


http://localhost:11768/checkCustomerWithTrans


http://127.0.0.1:5200/cap

how to add health check for consul
https://github.com/dotnetcore/CAP/issues/116

