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
using System;

namespace SME.SGP.Fechamento.Worker;

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

        registrarDependencias.RegistrarCasoDeUsoFechamentoRabbitSgp(
            builder.Services);

        // Serviços originalmente registrados no Program antigo
        builder.Services.AddHostedService<WorkerRabbitFechamento>();

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

        app.Run(async context =>
        {
            await context.Response.WriteAsync(
                "WorkerRabbitMQFechamento!");
        });
    }
}