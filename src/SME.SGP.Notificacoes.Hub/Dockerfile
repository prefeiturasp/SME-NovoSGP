#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/SME.SGP.Notificacoes.Hub/SME.SGP.Notificacoes.Hub.csproj", "src/SME.SGP.Notificacoes.Hub/"]
RUN dotnet restore "src/SME.SGP.Notificacoes.Hub/SME.SGP.Notificacoes.Hub.csproj"
COPY . .
WORKDIR "/src/src/SME.SGP.Notificacoes.Hub"
RUN dotnet build "SME.SGP.Notificacoes.Hub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SGP.Notificacoes.Hub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SME.SGP.Notificacoes.Hub.dll"]