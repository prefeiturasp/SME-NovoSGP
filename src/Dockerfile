FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SME.SGP.Api/SME.SGP.Api.csproj", "src/SME.SGP.Api/"]
COPY ["SME.SGP.Dto/SME.SGP.Dto.csproj", "src/SME.SGP.Dto/"]
COPY ["SME.SGP.IoC/SME.SGP.IoC.csproj", "src/SME.SGP.IoC/"]
COPY ["SME.SGP.Dados/SME.SGP.Dados.csproj", "src/SME.SGP.Dados/"]
COPY ["SME.SGP.Dominio/SME.SGP.Dominio.csproj", "src/SME.SGP.Dominio/"]
COPY ["SME.SGP.Aplicacao/SME.SGP.Aplicacao.csproj", "src/SME.SGP.Aplicacao/"]
RUN dotnet restore "src/SME.SGP.Api/SME.SGP.Api.csproj"
COPY . .
WORKDIR "/src/SME.SGP.Api"
RUN dotnet build "SME.SGP.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SME.SGP.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SME.SGP.Api.dll"]