Local

docker pull docker.elastic.co/elasticsearch/elasticsearch-oss:7.10.2

```bash
MEM=75 && \
    docker rm -f elasticsearch && \
    docker run -d --name elasticsearch -p 9200:9200 \
        -e "discovery.type=single-node" \
        -e "ES_JAVA_OPTS=-Xms${MEM}m -Xmx${MEM}m" \
        docker.elastic.co/elasticsearch/elasticsearch-oss:7.10.2 && \
    START=$(date +%s) && \
    (docker logs -t -f elasticsearch &) | grep -m1 starting && \
    echo $(($(date +%s)-START))
```


```bash
MEM=96 && \
    docker rm -f elasticsearch && \
    docker run -d --name elasticsearch -p 9200:9200 \
        -e "discovery.type=single-node" \
        -e "ES_JAVA_OPTS=-Xms${MEM}m -Xmx${MEM}m" \
        elasticsearch:7.10.1 && \
    START=$(date +%s) && \
    (docker logs -t -f elasticsearch &) | grep -m1 starting && \
    echo $(($(date +%s)-START))
```

Cluster
```bash
kubectl apply -f elasticsearch-deployment.yaml && \
    POD=$(kubectl get pod -l app=elasticsearch -o jsonpath="{.items[0].metadata.name}") && \
    echo ${POD} && \
    kubectl wait --for=condition=ready pod ${POD} && \
    START=$(date +%s) && \
    (kubectl logs -f ${POD} &) | grep -m1 starting && echo $(($(date +%s)-START))
```

Kibana

```bash
docker rm -f kibana && \
    docker run -d --link elasticsearch:elasticsearch \
        --name kibana -p 5601:5601 docker.elastic.co/kibana/kibana:7.10.2
```


localhost: 38s
cluster (unlimited): 37s
cluster (max cpu 500m): 68s
cluster (max cpu 250m): 142s


Errors

```json
{
   "error":{
      "root_cause":[
         {
            "type":"circuit_breaking_exception",
            "reason":"[parent] Data too large, data for [<http_request>] would be [76010944/72.4mb], which is larger than the limit of [75707187/72.1mb], real usage: [76010944/72.4mb], new bytes reserved: [0/0b], usages [request=0/0b, fielddata=0/0b, in_flight_requests=752/752b, model_inference=0/0b, accounting=0/0b]",
            "bytes_wanted":76010944,
            "bytes_limit":75707187,
            "durability":"TRANSIENT"
         }
      ],
      "type":"circuit_breaking_exception",
      "reason":"[parent] Data too large, data for [<http_request>] would be [76010944/72.4mb], which is larger than the limit of [75707187/72.1mb], real usage: [76010944/72.4mb], new bytes reserved: [0/0b], usages [request=0/0b, fielddata=0/0b, in_flight_requests=752/752b, model_inference=0/0b, accounting=0/0b]",
      "bytes_wanted":76010944,
      "bytes_limit":75707187,
      "durability":"TRANSIENT"
   },
   "status":429
}
```
