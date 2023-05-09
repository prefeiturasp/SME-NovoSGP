using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Auditoria.Worker;
using SME.SGP.ComprimirArquivos.Worker;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.IoC;

namespace SME.SGP.ComprimirArquivos.Worker
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
            RegistrarMediator(services);
            RegistrarDependencias(services);
            RegistrarRabbitMQ(services);
            RegistrarTelemetria(services);

            services.AddHealthChecks();
            
            services.AddHealthChecksUiSgp();
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

        private void RegistrarDependencias(IServiceCollection services)
        {
            services.ConfigurarTelemetria(Configuration);
            services.TryAddScoped<IOtimizarImagensUseCase, OtimizarImagemUseCase>();
            services.TryAddScoped<IOtimizarVideoUseCase, OtimizarVideoUseCase>();
        }

        private static void RegistrarMediator(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
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
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constantes.Arquivos)),
                RequestPath = $"/{Constantes.Arquivos}",
            });
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constantes.Temp)),
                RequestPath = $"/{Constantes.Temp}",
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitOtimizarArquivos!");
            });
        }
    }
}
