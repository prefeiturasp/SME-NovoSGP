#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SME.SGP.Notificacoes.Worker/SME.SGP.Notificacoes.Worker.csproj", "src/SME.SGP.Notificacoes.Worker/"]
RUN dotnet restore "src/SME.SGP.Notificacoes.Worker/SME.SGP.Notificacoes.Worker.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.Notificacoes.Worker"
RUN dotnet build "SME.SGP.Notificacoes.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.Notificacoes.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.Notificacoes.Worker.dll"]