FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["src/SME.SGP.Worker.Rabbbit/SME.SGP.Worker.Rabbbit.csproj", "src/SME.SGP.Worker.Rabbbit/"]
COPY ["src/SME.SGP.Infra/SME.SGP.Infra.csproj", "src/SME.SGP.Infra/"]
COPY ["src/SME.SGP.Dominio/SME.SGP.Dominio.csproj", "src/SME.SGP.Dominio/"]
COPY ["src/SME.SGP.IoC/SME.SGP.IoC.csproj", "src/SME.SGP.IoC/"]
COPY ["src/SME.SGP.Dominio.Servicos/SME.SGP.Dominio.Servicos.csproj", "src/SME.SGP.Dominio.Servicos/"]
COPY ["src/SME.SGP.Dados/SME.SGP.Dados.csproj", "src/SME.SGP.Dados/"]
COPY ["src/SME.SGP.Dominio.Interfaces/SME.SGP.Dominio.Interfaces.csproj", "src/SME.SGP.Dominio.Interfaces/"]
COPY ["src/SME.SGP.Aplicacao/SME.SGP.Aplicacao.csproj", "src/SME.SGP.Aplicacao/"]

RUN dotnet restore "src/SME.SGP.Worker.Rabbbit/SME.SGP.Worker.Rabbbit.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.Worker.Rabbbit"
RUN dotnet build "SME.SGP.Worker.Rabbbit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.Worker.Rabbbit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

RUN sed -i 's|http://deb.debian.org/debian|http://archive.debian.org/debian|g' /etc/apt/sources.list && \
    sed -i 's|http://security.debian.org/debian-security|http://archive.debian.org/debian-security|g' /etc/apt/sources.list && \
    echo 'Acquire::Check-Valid-Until "false";' > /etc/apt/apt.conf.d/99no-check-valid-until && \
    apt-get update && \
    apt-get upgrade -y && \
    update-ca-certificates --fresh

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SME.SGP.Worker.Rabbbit.dll"]
