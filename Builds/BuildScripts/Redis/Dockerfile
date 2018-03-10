FROM redis:4
MAINTAINER RiCo
RUN chown redis:redis /data
VOLUME /data
WORKDIR /data
RUN mkdir -p /usr/local/etc/redis && chmod ugo=+rwx /usr/local/etc/redis
COPY redis-master.conf /usr/local/etc/redis/redis.conf
EXPOSE 6379
CMD ["redis-server","/usr/local/etc/redis/redis.conf"]