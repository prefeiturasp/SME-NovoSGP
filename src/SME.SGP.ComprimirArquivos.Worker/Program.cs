using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.OtimizarArquivos.Worker.Interfaces;
using SME.SGP.OtimizarArquivos.Worker.UseCases;
using System;
using System.Reflection;

namespace SME.SGP.ComprimirArquivos.Worker;

public class Program
{
    protected Program() { }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurações
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets<Program>();

        AppContext.SetSwitch(
            "Npgsql.EnableLegacyTimestampBehavior",
            true);

        var assembly = Assembly.GetExecutingAssembly();

        // MediatR e FluentValidation
        RegistrarMediator(
            builder.Services,
            assembly);

        // Dependências da aplicação
        RegistrarDependencias(
            builder.Services,
            builder.Configuration);

        RegistrarRabbitMQ(
            builder.Services,
            builder.Configuration);

        RegistrarRabbitMQLog(
            builder.Services,
            builder.Configuration);

        RegistrarTelemetria(
            builder.Services,
            builder.Configuration);

        // Health checks
        builder.Services.AddHealthChecks();
        builder.Services.AddHealthChecksUiSgp();

        // Worker
        builder.Services.AddHostedService<WorkerRabbitComprimirArquivos>();

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

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync(
                "WorkerRabbitOtimizarArquivos!");
        });
        app.Run();
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
    }

    private static void RegistrarRabbitMQ(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<ConfiguracaoRabbitOptions>()
            .Bind(
                configuration.GetSection(
                    ConfiguracaoRabbitOptions.Secao),
                options =>
                    options.BindNonPublicProperties = true);

        services.AddSingleton<IConnectionFactory>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<
                    IOptions<ConfiguracaoRabbitOptions>>()
                .Value;

            return new ConnectionFactory
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = options.VirtualHost,
                RequestedHeartbeat =
                    TimeSpan.FromSeconds(
                        options.TempoHeartBeat)
            };
        });
    }

    private static void RegistrarRabbitMQLog(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<ConfiguracaoRabbitLogOptions>()
            .Bind(
                configuration.GetSection(
                    ConfiguracaoRabbitLogOptions.Secao),
                options =>
                    options.BindNonPublicProperties = true);

        services.AddSingleton<ConfiguracaoRabbitLogOptions>();

        services.AddSingleton<IConexoesRabbitFilasLog>(
            serviceProvider =>
            {
                var options = serviceProvider
                    .GetRequiredService<
                        IOptions<ConfiguracaoRabbitLogOptions>>()
                    .Value;

                var provider = serviceProvider
                                   .GetService<ObjectPoolProvider>()
                               ?? new DefaultObjectPoolProvider();

                return new ConexoesRabbitFilasLog(
                    options,
                    provider);
            });
    }

    private static void RegistrarDependencias(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigurarTelemetria(configuration);

        services
            .AddOptions<ConfiguracaoArmazenamentoOptions>()
            .Bind(
                configuration.GetSection(
                    ConfiguracaoArmazenamentoOptions.Secao),
                options =>
                    options.BindNonPublicProperties = true);

        services
            .AddOptions<ConfiguracaoRabbitOptions>()
            .Bind(
                configuration.GetSection(
                    ConfiguracaoRabbitOptions.Secao),
                options =>
                    options.BindNonPublicProperties = true);

        services.TryAddSingleton<IConexoesRabbitFilasSGP>(
            serviceProvider =>
            {
                var options = serviceProvider
                    .GetRequiredService<
                        IOptions<ConfiguracaoRabbitOptions>>()
                    .Value;

                var provider = serviceProvider
                    .GetService<ObjectPoolProvider>()
                    ?? new DefaultObjectPoolProvider();

                return new ConexoesRabbitFilasSGP(
                    options,
                    provider);
            });

        var policyRegistry =
            new Polly.Registry.PolicyRegistry
            {
                {
                    "RetryPolicyFilasRabbit",
                    Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(
                            3,
                            retryAttempt =>
                                TimeSpan.FromSeconds(
                                    Math.Pow(
                                        2,
                                        retryAttempt)))
                }
            };

        services.AddSingleton<
            IReadOnlyPolicyRegistry<string>>(
            policyRegistry);

        services.TryAddScoped<
            IServicoMensageriaSGP,
            ServicoMensageriaSGP>();

        services.TryAddScoped<
            IServicoArmazenamento,
            ServicoArmazenamento>();

        services.TryAddScoped<
            IComprimirImagensUseCase,
            ComprimirImagemUseCase>();

        services.TryAddScoped<
            IComprimirVideoUseCase,
            ComprimirVideoUseCase>();

        services.TryAddScoped<
            IComprimirPdfUsecase,
            ComprimirPdfUsecase>();

        services.AddSingleton<CaminhoGhostscriptUtil>();
    }

    private static void RegistrarMediator(
        IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(
                assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidacoesPipeline<,>));

        AssemblyScanner
            .FindValidatorsInAssembly(assembly)
            .ForEach(result =>
                services.AddScoped(
                    result.InterfaceType,
                    result.ValidatorType));
    }
}