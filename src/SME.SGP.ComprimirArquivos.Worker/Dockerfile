FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update && \
    apt-get install -y ffmpeg ghostscript && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SME.SGP.ComprimirArquivos.Worker/SME.SGP.ComprimirArquivos.Worker.csproj", "src/SME.SGP.ComprimirArquivos.Worker/"]
RUN dotnet restore "src/SME.SGP.ComprimirArquivos.Worker/SME.SGP.ComprimirArquivos.Worker.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.ComprimirArquivos.Worker"
RUN dotnet build "SME.SGP.ComprimirArquivos.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.ComprimirArquivos.Worker.csproj" -c Release -o /app/publish /p:ErrorOnDuplicatePublishOutputFiles=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.ComprimirArquivos.Worker.dll"]