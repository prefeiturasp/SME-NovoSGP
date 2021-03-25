FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-bionic AS build
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
COPY --from=publish /app .
RUN mkdir Imagens
ENTRYPOINT ["dotnet", "SME.SGP.Api.dll"]

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/* 

