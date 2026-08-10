using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SME.SGP.Auditoria.Worker.Interfaces;
using SME.SGP.Auditoria.Worker.Mapeamentos;
using SME.SGP.Auditoria.Worker.Repositorio;
using SME.SGP.Auditoria.Worker.Repositorio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using System;
using System.Threading;

namespace SME.SGP.Auditoria.Worker;

public class Program
{
    protected Program()
    {
        
    }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets<Program>();

        // Configurações gerais
        MapAuditoriaRegistry.Initialize();

        AppContext.SetSwitch(
            "Npgsql.EnableLegacyTimestampBehavior",
            true);

        // Serviços originalmente registrados no Startup
        RegistrarElasticSearch(
            builder.Services,
            builder.Configuration);

        RegistrarDependencias(
            builder.Services,
            builder.Configuration);

        RegistrarMapeamentos();

        RegistrarTelemetria(
            builder.Services,
            builder.Configuration);

        ConfigurarConsumoFilas(
            builder.Services,
            builder.Configuration);

        RegistrarRabbitMQ(
            builder.Services,
            builder.Configuration);

        // Serviços originalmente registrados no Program antigo
        builder.Services.AddHostedService<WorkerRabbitAuditoria>();

        builder.Services
            .AddHealthChecks()
            .AddElasticSearchSgp();

        builder.Services.AddHealthChecksUiSgp();

        var app = builder.Build();

        // Elastic APM
        app.UseElasticApm(
            builder.Configuration,
            new SqlClientDiagnosticSubscriber(),
            new HttpDiagnosticsSubscriber());

        // Health checks
        app.UseHealthChecksSgp();
        app.UseHealthCheckPrometheusSgp();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Configuração do ThreadPool
        var threadPoolOptions = new ThreadPoolOptions();

        builder.Configuration
            .GetSection(ThreadPoolOptions.Secao)
            .Bind(
                threadPoolOptions,
                options =>
                    options.BindNonPublicProperties = true);

        if (threadPoolOptions.WorkerThreads > 0 &&
            threadPoolOptions.CompletionPortThreads > 0)
        {
            ThreadPool.SetMinThreads(
                threadPoolOptions.WorkerThreads,
                threadPoolOptions.CompletionPortThreads);
        }

        app.Run(async context =>
        {
            await context.Response.WriteAsync(
                "WorkerRabbitAuditoria!");
        });
    }

    private static void RegistrarElasticSearch(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegistrarElastic(configuration);
    }

    private static void RegistrarTelemetria(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<TelemetriaOptions>()
            .Bind(
                configuration.GetSection(
                    TelemetriaOptions.Secao),
                options =>
                    options.BindNonPublicProperties = true);

        services.AddSingleton<TelemetriaOptions>();
        services.AddSingleton<
            IServicoTelemetria,
            ServicoTelemetria>();
    }

    private static void ConfigurarConsumoFilas(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<ConsumoFilasOptions>()
            .Bind(
                configuration.GetSection("ConsumoFilas"),
                options =>
                    options.BindNonPublicProperties = true);

        services.AddSingleton<ConsumoFilasOptions>();
    }

    private static void RegistrarRabbitMQ(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPolicies();

        services.ConfigurarRabbit(configuration);
        services.ConfigurarRabbitParaLogs(configuration);
    }

    private static void RegistrarMapeamentos()
    {
        _ = new AuditoriaMap();
    }

    private static void RegistrarDependencias(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigurarTelemetria(configuration);

        services.TryAddScoped<
            IRepositorioAuditoria,
            RepositorioAuditoria>();

        services.TryAddScoped<
            IRegistrarAuditoriaUseCase,
            RegistrarAuditoriaUseCase>();
    }
}