FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SME.SGP.PainelEducacional.Worker/SME.SGP.PainelEducacional.Worker.csproj", "src/SME.SGP.PainelEducacional.Worker/"]
RUN dotnet restore "src/SME.SGP.PainelEducacional.Worker/SME.SGP.PainelEducacional.Worker.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.PainelEducacional.Worker"
RUN dotnet build "SME.SGP.PainelEducacional.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.PainelEducacional.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.PainelEducacional.Worker.dll"]