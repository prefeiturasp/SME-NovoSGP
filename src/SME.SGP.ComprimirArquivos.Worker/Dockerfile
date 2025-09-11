#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

RUN sed -i 's|deb.debian.org/debian|archive.debian.org/debian|g' /etc/apt/sources.list \
    && sed -i 's|security.debian.org/debian-security|archive.debian.org/debian-security|g' /etc/apt/sources.list \
    && sed -i '/updates/d' /etc/apt/sources.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends ffmpeg \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SME.SGP.ComprimirArquivos.Worker/SME.SGP.ComprimirArquivos.Worker.csproj", "src/SME.SGP.ComprimirArquivos.Worker/"]
RUN dotnet restore "src/SME.SGP.ComprimirArquivos.Worker/SME.SGP.ComprimirArquivos.Worker.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.ComprimirArquivos.Worker"
RUN dotnet build "SME.SGP.ComprimirArquivos.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.ComprimirArquivos.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.ComprimirArquivos.Worker.dll"]
