using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using System;
using System.Threading;

namespace SME.SGP.Notificacoes.Worker;

public class Program
{
    protected Program() { }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurações
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets<Program>();

        // Serviços originalmente registrados no Startup
        AppContext.SetSwitch(
            "Npgsql.EnableLegacyTimestampBehavior",
            true);

        builder.Services.AddPolicies();

        builder.Services.ConfigurarRabbit(
            builder.Configuration);

        builder.Services.ConfigurarTelemetria(
            builder.Configuration);

        RegistrarHub(
            builder.Services,
            builder.Configuration);

        // Serviços originalmente registrados no Program antigo
        builder.Services.AddHostedService<WorkerRabbitNotificacao>();

        builder.Services.AddHealthChecks();
        builder.Services.AddHealthChecksUiSgp();

        var app = builder.Build();

        // Elastic APM
        app.UseElasticApm(
            builder.Configuration,
            new HttpDiagnosticsSubscriber());

        // Health checks
        app.UseHealthChecksSgp();
        app.UseHealthCheckPrometheusSgp();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

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

        app.MapGet("/",   async context =>
        {
            await context.Response.WriteAsync(
                "WorkerRabbitNotificacoes!");
        });
        app.Run();
    }

    private static void RegistrarHub(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<HubOptions>()
            .Bind(
                configuration.GetSection(HubOptions.Secao),
                options =>
                    options.BindNonPublicProperties = true);

        services.AddSingleton<HubOptions>();

        services.AddSingleton<
            INotificacaoSgpHub,
            NotificacaoSgpHub>();
    }
}