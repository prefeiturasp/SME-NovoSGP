FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/SME.SGP.WorkerService/SME.SGP.Worker.Service.csproj", "src/SME.SGP.WorkerService/"]
COPY ["src/SME.SGP.IoC/SME.SGP.IoC.csproj", "src/SME.SGP.IoC/"]
COPY ["src/SME.SGP.Dominio.Servicos/SME.SGP.Dominio.Servicos.csproj", "src/SME.SGP.Dominio.Servicos/"]
COPY ["src/SME.SGP.Dados/SME.SGP.Dados.csproj", "src/SME.SGP.Dados/"]
COPY ["src/SME.SGP.Dominio.Interfaces/SME.SGP.Dominio.Interfaces.csproj", "src/SME.SGP.Dominio.Interfaces/"]
COPY ["src/SME.SGP.Infra/SME.SGP.Infra.csproj", "src/SME.SGP.Infra/"]
COPY ["src/SME.SGP.Dominio/SME.SGP.Dominio.csproj", "src/SME.SGP.Dominio/"]
COPY ["src/SME.SGP.Aplicacao/SME.SGP.Aplicacao.csproj", "src/SME.SGP.Aplicacao/"]
COPY ["src/Bibliotecas/SME.Background.Core/SME.Background.Core.csproj", "src/Bibliotecas/SME.Background.Core/"]
COPY ["src/Bibliotecas/SME.Background.Hangfire/SME.Background.Hangfire.csproj", "src/Bibliotecas/SME.Background.Hangfire/"]
COPY ["src/Bibliotecas/SME.SGP.Background/SME.SGP.Background.csproj", "src/Bibliotecas/SME.SGP.Background/"]
COPY ["src/SME.SGP.Api/SME.SGP.Api.csproj", "src/SME.SGP.Api/"]
RUN dotnet restore "src/SME.SGP.WorkerService/SME.SGP.Worker.Service.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.WorkerService"
RUN dotnet build "SME.SGP.Worker.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.Worker.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.Worker.Service.dll"]