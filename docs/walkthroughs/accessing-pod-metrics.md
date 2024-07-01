# Accessing Pod Metrics

This page contains a list of common `kubectl` commands

Options for `TARGET_NAMESPACE` are:

1. platformsvc
1. hostsvc
1. payload-app

Options for `TARGET_APP` in namespaces are:

1. platformsvc
    - platform-mts
    - platform-deployment
1. hostsvc
    - hostsvc-sensor
    - hostsvc-position
    - hostsvc-logging
    - hostsvc-link
1. payload-app
    - < your payload-app id>

```bash
## RUN THESE COMMANDS FIRST TO SET THE VARIABLES USED BY THE METRICS COMMANDS
# ------------ START ------------------
## Update to use the namespace from the list above
TARGET_NAMESPACE="platformsvc"

## Update to match the applicable app ID from the list above
TARGET_APP="platform-mts"

## Query for the pod name
TARGET_POD=$(kubectl get pods -n ${TARGET_NAMESPACE} -l app=${TARGET_APP} --sort-by=.metadata.creationTimestamp -o jsonpath='{.items[-1:].metadata.name}')
# ------------- END -----------------



## Run status - single pod
kubectl get pods -n ${TARGET_NAMESPACE} ${TARGET_POD} -o jsonpath='Name: {.metadata.name} Status: {.status.phase}{"\n"}'

## Run status - all pods in namespace
kubectl get pods -n ${TARGET_NAMESPACE} -o=jsonpath='{range .items[*]} Name: {.metadata.name}{"\t"}{"\t"} Status: {.status.phase}{"\n"}{end}'

## Last execution - single pod
kubectl get pods -n ${TARGET_NAMESPACE} ${TARGET_POD} -o custom-columns=NAME:.metadata.name,FINISHED:.status.containerStatuses[*].lastState.terminated.finishedAt

## Last execution - all pods in namespace
kubectl get pods -n ${TARGET_NAMESPACE} -o custom-columns=NAME:.metadata.name,FINISHED:.status.containerStatuses[*].lastState.terminated.finishedAt

## Update - single pod
kubectl get pod -n ${TARGET_NAMESPACE} ${TARGET_POD} -o jsonpath='Name: {.metadata.name}{"\t"}{"\t"} Start Time: {.status.startTime} Creation Time: {.metadata.creationTimestamp}{"\n"}'

## Restart / crash count - single pod
kubectl get pods -n ${TARGET_NAMESPACE} ${TARGET_POD} --sort-by='.status.containerStatuses[0].restartCount'

## Restart / crash count - all pods in namespace
kubectl get pods -n ${TARGET_NAMESPACE} --sort-by='.status.containerStatuses[0].restartCount'

## CPU & Memory usage - single pod
kubectl top pods -n ${TARGET_NAMESPACE} ${TARGET_POD}

## CPU & Memory usage - all pods in namespace
kubectl top pods -n ${TARGET_NAMESPACE}

## Describe pod
kubectl describe pods -l app=${TARGET_APP} -n ${TARGET_NAMESPACE}

## Get logs for a pod
kubectl logs -n ${TARGET_NAMESPACE} ${TARGET_POD} -c ${TARGET_APP} > "${TARGET_APP}.log"

## Trigger a redeploy
kubectl rollout restart deploy -n ${TARGET_NAMESPACE} ${TARGET_APP}
```
