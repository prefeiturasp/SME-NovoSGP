using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.SGP.Aplicacao;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;

namespace SME.SGP.Fechamento.Worker
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
            RegistraClientesHttp.Registrar(services, Configuration);

            services.AddApplicationInsightsTelemetry(Configuration);            

            var registrarDependencias = new RegistraDependencias();
            registrarDependencias.Registrar(services, Configuration);
            registrarDependencias.RegistrarPolicies(services);

            ConfiguraRabbitParaLogs(services);

            services.AddMemoryCache();
            services.AddHostedService<WorkerRabbitFechamento>();
        }

        private void ConfiguraRabbitParaLogs(IServiceCollection services)
        {
            var configuracaoRabbitLogOptions = new ConfiguracaoRabbitLogOptions();
            Configuration.GetSection("ConfiguracaoRabbitLog").Bind(configuracaoRabbitLogOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitLogOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitMQFechamento!");
            });
        }
    }
}
