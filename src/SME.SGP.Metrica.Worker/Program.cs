using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.Metrica.Worker.Repositorios;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace SME.SGP.Metrica.Worker;

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

        // Registros originalmente feitos no Startup
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

        RegistrarRabbitMQ(
            builder.Services,
            builder.Configuration);

        RegistrarConsumoFilas(
            builder.Services,
            builder.Configuration);

        // Registro do worker e dos health checks
        builder.Services.AddHostedService<WorkerRabbitMetrica>();

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
                "WorkerRabbitMetricas!");
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

        /*
         * Evita criar um segundo ServiceProvider durante o registro
         * dos serviços. A inicialização da telemetria deve ocorrer
         * preferencialmente após a construção do container.
         */
    }

    private static void RegistrarRabbitMQ(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPolicies();
        services.ConfigurarRabbit(configuration);
        services.ConfigurarRabbitParaLogs(configuration);
    }

    private static void RegistrarConsumoFilas(
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

    private static void RegistrarMapeamentos()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var tiposDeMapa = assembly
            .GetTypes()
            .Where(tipo =>
                tipo.IsClass &&
                !tipo.IsAbstract &&
                tipo.Name.EndsWith("Map"))
            .ToList();

        foreach (var tipo in tiposDeMapa)
        {
            Activator.CreateInstance(tipo);
        }
    }

    private static void RegistrarDependencias(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigurarTelemetria(configuration);

        services.AddHttpContextAccessor();

        services.AddMediatR(mediatorConfiguration =>
            mediatorConfiguration.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()));

        RegistrarRepositorio(services);
        RegistrarUseCases(services);
        AdicionarHttpClients(
            services,
            configuration);
    }

    private static void RegistrarRepositorio(
        IServiceCollection services)
    {
        services.TryAddScoped<
            IContextoAplicacao,
            ContextoHttp>();

        services.TryAddScoped<
            ISgpContext,
            SgpContext>();

        services.TryAddScoped<
            ISgpContextConsultas,
            SgpContextConsultas>();

        // PostgreSQL
        services.TryAddScoped<
            IRepositorioSGP,
            RepositorioSGP>();

        services.TryAddScoped<
            IRepositorioSGPConsulta,
            RepositorioSGPConsulta>();

        // ElasticSearch
        services.TryAddScoped<
            IRepositorioAcessos,
            RepositorioAcessos>();

        services.TryAddScoped<
            IRepositorioConselhoClasseDuplicado,
            RepositorioConselhoClasseDuplicado>();

        services.TryAddScoped<
            IRepositorioConselhoClasseAlunoDuplicado,
            RepositorioConselhoClasseAlunoDuplicado>();

        services.TryAddScoped<
            IRepositorioConselhoClasseNotaDuplicado,
            RepositorioConselhoClasseNotaDuplicado>();

        services.TryAddScoped<
            IRepositorioFechamentoTurmaDuplicado,
            RepositorioFechamentoTurmaDuplicado>();

        services.TryAddScoped<
            IRepositorioFechamentoTurmaDisciplinaDuplicado,
            RepositorioFechamentoTurmaDisciplinaDuplicado>();

        services.TryAddScoped<
            IRepositorioFechamentoAlunoDuplicado,
            RepositorioFechamentoAlunoDuplicado>();

        services.TryAddScoped<
            IRepositorioFechamentoNotaDuplicado,
            RepositorioFechamentoNotaDuplicado>();

        services.TryAddScoped<
            IRepositorioConsolidacaoConselhoClasseNotaNulos,
            RepositorioConsolidacaoConselhoClasseNotaNulos>();

        services.TryAddScoped<
            IRepositorioConsolidacaoConselhoClasseAlunoTurmaDuplicado,
            RepositorioConsolidacaoConselhoClasseAlunoTurmaDuplicado>();

        services.TryAddScoped<
            IRepositorioConsolidacaoCCNotaDuplicado,
            RepositorioConsolidacaoCCNotaDuplicado>();

        services.TryAddScoped<
            IRepositorioConselhoClasseNaoConsolidado,
            RepositorioConselhoClasseNaoConsolidado>();

        services.TryAddScoped<
            IRepositorioFrequenciaAlunoInconsistente,
            RepositorioFrequenciaAlunoInconsistente>();

        services.TryAddScoped<
            IRepositorioFrequenciaAlunoDuplicado,
            RepositorioFrequenciaAlunoDuplicado>();

        services.TryAddScoped<
            IRepositorioRegistroFrequenciaDuplicado,
            RepositorioRegistroFrequenciaDuplicado>();

        services.TryAddScoped<
            IRepositorioRegistroFrequenciaAlunoDuplicado,
            RepositorioRegistroFrequenciaAlunoDuplicado>();

        services.TryAddScoped<
            IRepositorioConsolidacaoFrequenciaAlunoMensalInconsistente,
            RepositorioConsolidacaoFrequenciaAlunoMensalInconsistente>();

        services.TryAddScoped<
            IRepositorioDiarioBordoDuplicado,
            RepositorioDiarioBordoDuplicado>();

        services.TryAddScoped<
            IRepositorioRegistrosFrequenciaDiario,
            RepositorioRegistrosFrequenciaDiario>();

        services.TryAddScoped<
            IRepositorioDiariosBordoDiario,
            RepositorioDiariosBordoDiario>();

        services.TryAddScoped<
            IRepositorioDevolutivasDiarioBordoMensal,
            RepositorioDevolutivasDiarioBordoMensal>();

        services.TryAddScoped<
            IRepositorioAulasCJMensal,
            RepositorioAulasCJMensal>();

        services.TryAddScoped<
            IRepositorioEncaminhamentosAEEMensal,
            RepositorioEncaminhamentosAEEMensal>();

        services.TryAddScoped<
            IRepositorioPlanosAEEMensal,
            RepositorioPlanosAEEMensal>();

        services.TryAddScoped<
            IRepositorioPlanosAulaDiario,
            RepositorioPlanosAulaDiario>();

        services.TryAddScoped<
            IRepositorioDevolutivaDuplicado,
            RepositorioDevolutivaDuplicado>();

        services.TryAddScoped<
            IRepositorioDevolutivaMaisDeUmaNoDiario,
            RepositorioDevolutivaMaisDeUmaNoDiario>();

        services.TryAddScoped<
            IRepositorioDevolutivaSemDiario,
            RepositorioDevolutivaSemDiario>();

        services.TryAddScoped<
            IRepositorioFechamentosNotaDiario,
            RepositorioFechamentosNotaDiario>();

        services.TryAddScoped<
            IRepositorioConselhosClasseAlunoDiario,
            RepositorioConselhosClasseAlunoDiario>();

        services.TryAddScoped<
            IRepositorioFechamentosTurmaDisciplinaDiario,
            RepositorioFechamentosTurmaDisciplinaDiario>();

        services.TryAddScoped<
            IRepositorioAulasSemAtribuicaoSubstituicaoMensal,
            RepositorioAulasSemAtribuicaoSubstituicaoMensal>();
    }

    private static void RegistrarUseCases(
        IServiceCollection services)
    {
        services.TryAddScoped<
            IAcessosDiarioSGPUseCase,
            AcessosDiarioSGPUseCase>();

        services.TryAddScoped<
            IConselhoClasseDuplicadoUseCase,
            ConselhoClasseDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaConselhoClasseDuplicadoUseCase,
            LimpezaConselhoClasseDuplicadoUseCase>();

        services.TryAddScoped<
            IConselhoClasseAlunoDuplicadoUseCase,
            ConselhoClasseAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IConselhoClasseAlunoUeDuplicadoUseCase,
            ConselhoClasseAlunoUeDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaConselhoClasseAlunoDuplicadoUseCase,
            LimpezaConselhoClasseAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IConselhoClasseNotaDuplicadoUseCase,
            ConselhoClasseNotaDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaConselhoClasseNotaDuplicadoUseCase,
            LimpezaConselhoClasseNotaDuplicadoUseCase>();

        services.TryAddScoped<
            IFechamentoTurmaDuplicadoUseCase,
            FechamentoTurmaDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaFechamentoTurmaDuplicadoUseCase,
            LimpezaFechamentoTurmaDuplicadoUseCase>();

        services.TryAddScoped<
            IFechamentoTurmaDisciplinaDuplicadoUseCase,
            FechamentoTurmaDisciplinaDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaFechamentoTurmaDisciplinaDuplicadoUseCase,
            LimpezaFechamentoTurmaDisciplinaDuplicadoUseCase>();

        services.TryAddScoped<
            IFechamentoAlunoDuplicadoUseCase,
            FechamentoAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IFechamentoAlunoDuplicadoUEUseCase,
            FechamentoAlunoDuplicadoUEUseCase>();

        services.TryAddScoped<
            ILimpezaFechamentoAlunoDuplicadoUseCase,
            LimpezaFechamentoAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IFechamentoNotaDuplicadoUseCase,
            FechamentoNotaDuplicadoUseCase>();

        services.TryAddScoped<
            IFechamentoNotaDuplicadoTurmaUseCase,
            FechamentoNotaDuplicadoTurmaUseCase>();

        services.TryAddScoped<
            ILimpezaFechamentoNotaDuplicadoUseCase,
            LimpezaFechamentoNotaDuplicadoUseCase>();

        services.TryAddScoped<
            IConsolidacaoConselhoClasseNotaNuloUseCase,
            ConsolidacaoConselhoClasseNotaNuloUseCase>();

        services.TryAddScoped<
            IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase,
            ConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase>();

        services.TryAddScoped<
            IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase,
            ConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase>();

        services.TryAddScoped<
            ILimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase,
            LimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase>();

        services.TryAddScoped<
            IConsolidacaoCCNotaDuplicadoUseCase,
            ConsolidacaoCCNotaDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaConsolidacaoCCNotaDuplicadoUseCase,
            LimpezaConsolidacaoCCNotaDuplicadoUseCase>();

        services.TryAddScoped<
            IConselhoClasseNaoConsolidadoUseCase,
            ConselhoClasseNaoConsolidadoUseCase>();

        services.TryAddScoped<
            IConselhoClasseNaoConsolidadoUEUseCase,
            ConselhoClasseNaoConsolidadoUEUseCase>();

        services.TryAddScoped<
            IFrequenciaAlunoInconsistenteUseCase,
            FrequenciaAlunoInconsistenteUseCase>();

        services.TryAddScoped<
            IFrequenciaAlunoInconsistenteUEUseCase,
            FrequenciaAlunoInconsistenteUEUseCase>();

        services.TryAddScoped<
            IFrequenciaAlunoInconsistenteTurmaUseCase,
            FrequenciaAlunoInconsistenteTurmaUseCase>();

        services.TryAddScoped<
            IFrequenciaAlunoDuplicadoUseCase,
            FrequenciaAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IFrequenciaAlunoDuplicadoUEUseCase,
            FrequenciaAlunoDuplicadoUEUseCase>();

        services.TryAddScoped<
            ILimpezaFrequenciaAlunoDuplicadoUseCase,
            LimpezaFrequenciaAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IRegistroFrequenciaDuplicadoUseCase,
            RegistroFrequenciaDuplicadoUseCase>();

        services.TryAddScoped<
            IRegistroFrequenciaDuplicadoUEUseCase,
            RegistroFrequenciaDuplicadoUEUseCase>();

        services.TryAddScoped<
            ILimpezaRegistroFrequenciaDuplicadoUseCase,
            LimpezaRegistroFrequenciaDuplicadoUseCase>();

        services.TryAddScoped<
            ILimpezaRegistroFrequenciaAlunoDuplicadoUseCase,
            LimpezaRegistroFrequenciaAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IRegistroFrequenciaAlunoDuplicadoUseCase,
            RegistroFrequenciaAlunoDuplicadoUseCase>();

        services.TryAddScoped<
            IRegistroFrequenciaAlunoDuplicadoUEUseCase,
            RegistroFrequenciaAlunoDuplicadoUEUseCase>();

        services.TryAddScoped<
            IRegistroFrequenciaAlunoDuplicadoTurmaUseCase,
            RegistroFrequenciaAlunoDuplicadoTurmaUseCase>();

        services.TryAddScoped<
            IConsolidacaoFrequenciaAlunoMensalInconsistenteUseCase,
            ConsolidacaoFrequenciaAlunoMensalInconsistenteUseCase>();

        services.TryAddScoped<
            IConsolidacaoFrequenciaAlunoMensalInconsistenteUEUseCase,
            ConsolidacaoFrequenciaAlunoMensalInconsistenteUEUseCase>();

        services.TryAddScoped<
            IConsolidacaoFrequenciaAlunoMensalInconsistenteTurmaUseCase,
            ConsolidacaoFrequenciaAlunoMensalInconsistenteTurmaUseCase>();

        services.TryAddScoped<
            IDiarioBordoDuplicadoUseCase,
            DiarioBordoDuplicadoUseCase>();

        services.TryAddScoped<
            IRegistrosFrequenciaDiarioUseCase,
            RegistrosFrequenciaDiarioUseCase>();

        services.TryAddScoped<
            IDiariosBordoDiarioUseCase,
            DiariosBordoDiarioUseCase>();

        services.TryAddScoped<
            IDevolutivasDiarioBordoMensalUseCase,
            DevolutivasDiarioBordoMensalUseCase>();

        services.TryAddScoped<
            IAulasCJMensalUseCase,
            AulasCJMensalUseCase>();

        services.TryAddScoped<
            IEncaminhamentosAEEMensalUseCase,
            EncaminhamentosAEEMensalUseCase>();

        services.TryAddScoped<
            IPlanosAEEMensalUseCase,
            PlanosAEEMensalUseCase>();

        services.TryAddScoped<
            IPlanosAulaDiarioUseCase,
            PlanosAulaDiarioUseCase>();

        services.TryAddScoped<
            IFechamentosNotaDiarioUseCase,
            FechamentosNotaDiarioUseCase>();

        services.TryAddScoped<
            IConselhosClasseAlunoDiarioUseCase,
            ConselhosClasseAlunoDiarioUseCase>();

        services.TryAddScoped<
            IFechamentosTurmaDisciplinaDiarioUseCase,
            FechamentosTurmaDisciplinaDiarioUseCase>();

        services.TryAddScoped<
            IAulasSemAtribuicaoSubstituicaoMensalUseCase,
            AulasSemAtribuicaoSubstituicaoMensalUseCase>();

        services.TryAddScoped<
            IAulasSemAtribuicaoSubstituicaoUEMensalUseCase,
            AulasSemAtribuicaoSubstituicaoUEMensalUseCase>();

        services.TryAddScoped<
            IAulasSemAtribuicaoSubstituicaoTurmaMensalUseCase,
            AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase>();

        services.TryAddScoped<
            IAulasSemAtribuicaoSubstituicaoComponenteMensalUseCase,
            AulasSemAtribuicaoSubstituicaoComponenteMensalUseCase>();

        services.TryAddScoped<
            IAulasSemAtribuicaoSubstituicaoExclusaoTurmaMensalUseCase,
            AulasSemAtribuicaoSubstituicaoExclusaoTurmaMensalUseCase>();

        services.TryAddScoped<
            IDevolutivaDuplicadoUseCase,
            DevolutivaDuplicadoUseCase>();

        services.TryAddScoped<
            IDevolutivaMaisDeUmaNoDiarioUseCase,
            DevolutivaMaisDeUmaNoDiarioUseCase>();

        services.TryAddScoped<
            IDevolutivaSemDiarioUseCase,
            DevolutivaSemDiarioUseCase>();
    }

    private static void AdicionarHttpClients(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var urlApiEol = configuration["UrlApiEOL"];
        var apiKeyEol = configuration["ApiKeyEolApi"];
        var timeout = configuration["HttpClientTimeoutSecond"];

        services
            .AddHttpClient(
                ServicosEolConstants.SERVICO,
                client =>
                {
                    client.BaseAddress = new Uri(urlApiEol);
                    client.DefaultRequestHeaders.Add(
                        "Accept",
                        "application/json");

                    client.DefaultRequestHeaders.Add(
                        "x-api-eol-key",
                        apiKeyEol);

                    if (timeout.NaoEhNulo())
                    {
                        client.Timeout = TimeSpan.FromSeconds(
                            double.Parse(timeout));
                    }
                })
            .AddPolicyHandler(GetRetryPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage>
        GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                2,
                retryAttempt =>
                    TimeSpan.FromSeconds(
                        Math.Pow(2, retryAttempt)));
    }
}