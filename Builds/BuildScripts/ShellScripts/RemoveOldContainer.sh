﻿#!/bin/sh
result=$(sudo docker ps -a -q --filter ancestor=coreapp)

if [[ -n "$result" ]]; then
  sudo docker stop $(sudo docker ps -a -q --filter ancestor=coreapp)
  echo "stopped old Containers of coreapp"
  sudo docker rm $(sudo docker ps -a -q --filter ancestor=coreapp)
  echo "removed old Containers of coreapp"
else
  echo "No such container"
fi