FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS build
WORKDIR /src
COPY ["src/SME.SGP.Worker.Rabbit/SME.SGP.Worker.Rabbit.csproj", "src/SME.SGP.Worker.Rabbit/"]
COPY ["src/SME.SGP.Infra/SME.SGP.Infra.csproj", "src/SME.SGP.Infra/"]
COPY ["src/SME.SGP.Dominio/SME.SGP.Dominio.csproj", "src/SME.SGP.Dominio/"]
COPY ["src/SME.SGP.IoC/SME.SGP.IoC.csproj", "src/SME.SGP.IoC/"]
COPY ["src/SME.SGP.Dominio.Servicos/SME.SGP.Dominio.Servicos.csproj", "src/SME.SGP.Dominio.Servicos/"]
COPY ["src/SME.SGP.Dados/SME.SGP.Dados.csproj", "src/SME.SGP.Dados/"]
COPY ["src/SME.SGP.Dominio.Interfaces/SME.SGP.Dominio.Interfaces.csproj", "src/SME.SGP.Dominio.Interfaces/"]
COPY ["src/SME.SGP.Aplicacao/SME.SGP.Aplicacao.csproj", "src/SME.SGP.Aplicacao/"]
COPY ["src/Bibliotecas/SME.Background.Core/SME.Background.Core.csproj", "src/Bibliotecas/SME.Background.Core/"]
RUN dotnet restore "src/SME.SGP.Worker.Rabbit/SME.SGP.Worker.Rabbit.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.Worker.Rabbit"
RUN dotnet build "SME.SGP.Worker.Rabbit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.Worker.Rabbit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.Worker.Rabbit.dll"]