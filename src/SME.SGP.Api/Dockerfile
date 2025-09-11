FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["src/SME.SGP.Api/SME.SGP.Api.csproj", "src/SME.SGP.Api/"]
COPY ["src/SME.SGP.Infra/SME.SGP.Infra.csproj", "src/SME.SGP.Infra/"]
COPY ["src/SME.SGP.IoC/SME.SGP.IoC.csproj", "src/SME.SGP.IoC/"]
COPY ["src/SME.SGP.Dados/SME.SGP.Dados.csproj", "src/SME.SGP.Dados/"]
COPY ["src/SME.SGP.Dominio/SME.SGP.Dominio.csproj", "src/SME.SGP.Dominio/"]
COPY ["src/SME.SGP.Aplicacao/SME.SGP.Aplicacao.csproj", "src/SME.SGP.Aplicacao/"]
COPY ["src/SME.SGP.Dominio.Interfaces/SME.SGP.Dominio.Interfaces.csproj", "src/SME.SGP.Dominio.Interfaces/"]

RUN dotnet restore "src/SME.SGP.Api/SME.SGP.Api.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.Api"
RUN dotnet build "SME.SGP.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SME.SGP.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app ./
RUN mkdir Imagens

ENV TZ=America/Sao_Paulo

# Corrigindo fontes do Debian Buster EOL
RUN sed -i 's|http://deb.debian.org|http://archive.debian.org|g' /etc/apt/sources.list \
    && sed -i '/security.debian.org/d' /etc/apt/sources.list \
    && apt-get update \
    && apt-get install -y --allow-unauthenticated \
        tzdata \
        locales \
        libc6-dev \
        libgdiplus \
        libx11-dev \
    && echo "en_US.UTF-8 UTF-8" > /etc/locale.gen \
    && locale-gen \
    && dpkg-reconfigure --frontend noninteractive tzdata \
    && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "SME.SGP.Api.dll"]
