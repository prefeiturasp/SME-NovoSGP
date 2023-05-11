using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Auditoria.Worker.Interfaces;
using SME.SGP.Auditoria.Worker.Repositorio;
using SME.SGP.Auditoria.Worker.Repositorio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using System.Threading;

namespace SME.SGP.Auditoria.Worker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegistrarElasticSearch(services);
            RegistrarDependencias(services);
            RegistrarMapeamentos();
            RegistrarRabbitMQ(services);
            RegistrarTelemetria(services);
        }

        private void RegistrarElasticSearch(IServiceCollection services)
        {
            services.RegistrarElastic(Configuration);
        }

        private void RegistrarTelemetria(IServiceCollection services)
        {
            services.AddOptions<TelemetriaOptions>()
                .Bind(Configuration.GetSection(TelemetriaOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<TelemetriaOptions>();
        }

        private void RegistrarRabbitMQ(IServiceCollection services)
        {
            services.AddOptions<ConfiguracaoRabbitOptions>()
                .Bind(Configuration.GetSection(ConfiguracaoRabbitOptions.Secao), c => c.BindNonPublicProperties = true);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitOptions>>().Value;

            services.AddSingleton<IConnectionFactory>(serviceProvider =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = options.HostName,
                    UserName = options.UserName,
                    Password = options.Password,
                    VirtualHost = options.VirtualHost,
                    RequestedHeartbeat = System.TimeSpan.FromSeconds(options.TempoHeartBeat),
                };

                return factory;
            });
            services.AddSingleton<ConfiguracaoRabbitOptions>();
        }

        private void RegistrarMapeamentos()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AuditoriaMap());

                config.ForDommel();
            });
        }

        private void RegistrarDependencias(IServiceCollection services)
        {
            services.ConfigurarTelemetria(Configuration);
            services.TryAddScoped<IRepositorioAuditoria, RepositorioAuditoria>();

            services.TryAddScoped<IRegistrarAuditoriaUseCase, RegistrarAuditoriaUseCase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            app.UseHealthChecksSgp();
            app.UseHealthCheckPrometheusSgp();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            var threadPoolOptions = new ThreadPoolOptions();
            Configuration.GetSection(ThreadPoolOptions.Secao).Bind(threadPoolOptions, c => c.BindNonPublicProperties = true);
            if (threadPoolOptions.WorkerThreads > 0 && threadPoolOptions.CompletionPortThreads > 0)
                ThreadPool.SetMinThreads(threadPoolOptions.WorkerThreads, threadPoolOptions.CompletionPortThreads);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitAuditoria!");
            });
        }
    }
}
