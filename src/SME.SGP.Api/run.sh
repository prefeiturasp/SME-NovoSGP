#!/bin/bash

# screen -d -m -S Redis /usr/bin/redis-server && dotnet SME.SGP.API.dll
# nohup redis-server && dotnet SME.SGP.API.dll

# Start Redis
/usr/bin/redis-server & 
status=$?
if [ $status -ne 0 ]; then
  echo "Failed to start redis-server: $status"
  exit $status
fi

# Start SGP
dotnet SME.SGP.Api.dll
