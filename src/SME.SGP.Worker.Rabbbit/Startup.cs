﻿using System;
using System.Net;
using System.Net.Http;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Sentry;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.Worker.RabbitMQ;


namespace SME.SGP.Worker.Rabbbit
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

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
            
            RegistraDependencias.Registrar(services);
            
            RegistrarHttpClients(services, configuration);
            
            services.AddApplicationInsightsTelemetry(configuration);         
            
            services.AddHostedService<WorkerRabbitMQ>();

            ConfiguraVariaveisAmbiente(services);

            var serviceProvider = services.BuildServiceProvider();
            var clientTelemetry = serviceProvider.GetService<TelemetryClient>();
            DapperExtensionMethods.Init(clientTelemetry);
            SentrySdk.Init(configuration.GetValue<string>("Sentry:DSN"));

            services.AddMemoryCache();
        }
        private void ConfiguraVariaveisAmbiente(IServiceCollection services)
        {
            var configuracaoRabbitOptions = new ConfiguracaoRabbitOptions();
            configuration.GetSection(nameof(ConfiguracaoRabbitOptions)).Bind(configuracaoRabbitOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitMQ!");
            });
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
            });

            services.AddHttpClient(name: "servicoServidorRelatorios", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlServidorRelatorios").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

        }   
    }
}