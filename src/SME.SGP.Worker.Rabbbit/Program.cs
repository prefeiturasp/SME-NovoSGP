using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.SGP.Infra;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;
using SME.SGP.Worker.RabbitMQ;
using System;

namespace SME.SGP.Worker.Rabbbit;

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

        var registrarDependencias = new RegistrarDependencias();

        registrarDependencias.RegistrarParaWorkers(
            builder.Services,
            builder.Configuration);

        registrarDependencias.RegistrarCasoDeUsoRabbitSgp(
            builder.Services);

        // Serviços originalmente registrados no Program antigo
        builder.Services.AddHostedService<WorkerRabbitMQ>();

        builder.Services.AddHealthChecks();
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

        RegistrarConfigsThreads.Registrar(
            builder.Configuration);

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync(
                "WorkerRabbitMQ!");
        });
        app.Run();
    }
}