export
nohup redis-server --bind '0.0.0.0' &> redis.log & dotnet SME.SGP.Api.dll
