FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN sed -i 's|http://deb.debian.org/debian|http://archive.debian.org/debian|g' /etc/apt/sources.list && \
    sed -i '/security/d' /etc/apt/sources.list && \
    rm -f /etc/apt/sources.list.d/* && \
    apt update && \
    apt install -y ffmpeg ghostscript && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SME.SGP.ComprimirArquivos.Worker.csproj", "SME.SGP.ComprimirArquivos.Worker/"]
RUN dotnet restore "SME.SGP.ComprimirArquivos.Worker/SME.SGP.ComprimirArquivos.Worker.csproj"
COPY . .
WORKDIR "/src/SME.SGP.ComprimirArquivos.Worker"
RUN dotnet build "SME.SGP.ComprimirArquivos.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.ComprimirArquivos.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.ComprimirArquivos.Worker.dll"]