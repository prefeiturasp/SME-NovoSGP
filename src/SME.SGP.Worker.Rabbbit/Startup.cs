﻿using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;
using SME.SGP.Worker.RabbitMQ;
using System;
using System.Net;
using System.Net.Http;


namespace SME.SGP.Worker.Rabbbit
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;
        private ConfiguracaoRabbitOptions configuracaoRabbitOptions;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            this.env = env ??
                throw new ArgumentNullException(nameof(env));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            RegistrarHttpClients(services, configuration);
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddPolicies();

            ConfiguraVariaveisAmbiente(services);

            var registrarDependencias = new RegistraDependencias();
            registrarDependencias.Registrar(services, configuracaoRabbitOptions);
            registrarDependencias.RegistrarGCA(services);

            ConfiguraGoogleClassroomSync(services);
            ConfiguraRabbitParaLogs(services);
            ConfiguraConsumoFilas(services);

            services.AddRabbit(configuracaoRabbitOptions);

            var telemetriaOptions = ConfiguraTelemetria(services);
            var serviceProvider = services.BuildServiceProvider();

            var clientTelemetry = serviceProvider.GetService<TelemetryClient>();

            var servicoTelemetria = new ServicoTelemetria(clientTelemetry, telemetriaOptions);

            DapperExtensionMethods.Init(servicoTelemetria);

            services.AddSingleton(servicoTelemetria);

            services.AddMemoryCache();

            services.AddHostedService<WorkerRabbitMQ>();
        }
        private TelemetriaOptions ConfiguraTelemetria(IServiceCollection services)
        {
            var telemetriaOptions = new TelemetriaOptions();
            configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(telemetriaOptions);

            return telemetriaOptions;
        }
        private void ConfiguraVariaveisAmbiente(IServiceCollection services)
        {
            configuracaoRabbitOptions = new ConfiguracaoRabbitOptions();
            configuration.GetSection(nameof(ConfiguracaoRabbitOptions)).Bind(configuracaoRabbitOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitOptions);
        }

        private void ConfiguraGoogleClassroomSync(IServiceCollection services)
        {
            var googleClassroomSyncOptions = new GoogleClassroomSyncOptions();
            configuration.GetSection(nameof(GoogleClassroomSyncOptions)).Bind(googleClassroomSyncOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(googleClassroomSyncOptions);
        }

        private void ConfiguraConsumoFilas(IServiceCollection services)
        {
            var consumoFilasOptions = new ConsumoFilasOptions();
            configuration.GetSection(ConsumoFilasOptions.Secao).Bind(consumoFilasOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(consumoFilasOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseElasticApm(configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitMQ!");
            });
        }
        private void ConfiguraRabbitParaLogs(IServiceCollection services)
        {
            var configuracaoRabbitLogOptions = new ConfiguracaoRabbitLogOptions();
            configuration.GetSection("ConfiguracaoRabbitLog").Bind(configuracaoRabbitLogOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitLogOptions);
        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3,
                                                                            retryAttempt)));
        }
        private static void RegistrarHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IServicoJurema, ServicoJurema>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiJurema").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<IServicoEol, ServicoEOL>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-api-eol-key", configuration.GetSection("ApiKeyEolApi").Value);
            });

            services.AddHttpClient(name: "servicoEOL", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-api-eol-key", configuration.GetSection("ApiKeyEolApi").Value);

            }).AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<IServicoAcompanhamentoEscolar, ServicoAcompanhamentoEscolar>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiAE").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient(name: "servicoAcompanhamentoEscolar", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiAE").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-integration-key", configuration.GetSection("AE_ChaveIntegracao").Value);
            });

            services.AddHttpClient<IServicoGithub, SevicoGithub>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiGithub").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            var cookieContainer = new CookieContainer();
            var jasperCookieHandler = new JasperCookieHandler() { CookieContainer = cookieContainer };

            services.AddSingleton(jasperCookieHandler);

            var basicAuth = $"{configuration.GetValue<string>("ConfiguracaoJasper:Username")}:{configuration.GetValue<string>("ConfiguracaoJasper:Password")}".EncodeTo64();
            var jasperUrl = configuration.GetValue<string>("ConfiguracaoJasper:Hostname");

            services.AddHttpClient<ISevicoJasper, SevicoJasper>(c =>
            {
                c.BaseAddress = new Uri(jasperUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuth}");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new JasperCookieHandler() { CookieContainer = cookieContainer };
            });

            services.AddHttpClient<IServicoServidorRelatorios, ServicoServidorRelatorios>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlServidorRelatorios").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-sr-api-key", configuration.GetSection("ApiKeySr").Value);
            });

            services.AddHttpClient(name: "servicoServidorRelatorios", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlServidorRelatorios").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-sr-api-key", configuration.GetSection("ApiKeySr").Value);
            });

        }
    }
}
