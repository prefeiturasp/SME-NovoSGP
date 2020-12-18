#!/bin/bash

# Start Redis
/usr/bin/redis-server & 
status=$?
if [ $status -ne 0 ]; then
  echo "Failed to start redis-server: $status"
  exit $status
fi

# Start SGP
dotnet SME.SGP.Api.dll
