using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.IoC;
using System;

namespace SME.SGP.Notificacoes.Hub;

public class Program
{
    public const string CustomTokenScheme = nameof(CustomTokenScheme);

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurações
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets<Program>();

        // Serviços
        AppContext.SetSwitch(
            "Npgsql.EnableLegacyTimestampBehavior",
            true);

        builder.Services.AddSignalR();

        builder.Services.ConfigurarTelemetria(
            builder.Configuration);

        builder.Services.AddPolicies();

        builder.Services.ConfigurarRabbitParaLogs(
            builder.Configuration);

        RegistrarCache(
            builder.Services,
            builder.Configuration);

        RegistrarEventosNotificacao(
            builder.Services);

        RegistrarAutenticacao(
            builder.Services);

        builder.Services.AddHealthChecks();
        builder.Services.AddHealthChecksUiSgp();

        var app = builder.Build();

        // Pipeline HTTP
        app.UseElasticApm(
            builder.Configuration,
            new HttpDiagnosticsSubscriber());

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

        app.UseAuthorization();

        app.MapHub<NotificacaoHub>("/notificacao");

        app.Run();
    }

    private static void RegistrarAutenticacao(
        IServiceCollection services)
    {
        services.AddAuthentication();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "Token",
                policy => policy
                    .AddAuthenticationSchemes(CustomTokenScheme)
                    .RequireClaim("token")
                    .RequireAuthenticatedUser());
        });
    }

    private static void RegistrarCache(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigurarMetricasCache();
        services.ConfigurarCache(configuration);

        services.AddSingleton<IRepositorioUsuario>(serviceProvider =>
        {
            var repositorioCache =
                serviceProvider.GetService<IRepositorioCache>();

            var repositorioUsuario =
                new RepositorioUsuario(repositorioCache);

            EventoNotificacaoExtensions.Inicializa(
                repositorioUsuario);

            return repositorioUsuario;
        });
    }

    private static void RegistrarEventosNotificacao(
        IServiceCollection services)
    {
        services.TryAddScoped<
            IEventoNotificacaoCriada,
            EventoNotificacaoCriada>();

        services.TryAddScoped<
            IEventoNotificacaoLida,
            EventoNotificacaoLida>();

        services.TryAddScoped<
            IEventoNotificacaoExcluida,
            EventoNotificacaoExcluida>();
    }
}