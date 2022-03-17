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
using Microsoft.OpenApi.Models;
using SME.SGP.Aplicacao;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;

namespace SME.SGP.Fechamento.Worker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }
        private ConfiguracaoRabbitOptions configuracaoRabbitOptions;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegistraClientesHttp.Registrar(services, configuration);
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddPolicies();
            ConfiguraVariaveisAmbiente(services);
            RegistraDependencias.Registrar(services, configuracaoRabbitOptions);

            ConfiguraRabbitParaLogs(services);
            ConfiguraTelemetria(services);

            services.AddRabbit(configuracaoRabbitOptions);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SME.SGP.Fechamento.Worker", Version = "v1" });
            });

            services.AddMemoryCache();
            services.AddHostedService<WorkerRabbitFechamento>();
        }

        private void ConfiguraRabbitParaLogs(IServiceCollection services)
        {
            var configuracaoRabbitLogOptions = new ConfiguracaoRabbitLogOptions();
            configuration.GetSection("ConfiguracaoRabbitLog").Bind(configuracaoRabbitLogOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitLogOptions);
        }

        private void ConfiguraTelemetria(IServiceCollection services)
        {
            var telemetriaOptions = new TelemetriaOptions();
            configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(telemetriaOptions);

            var serviceProvider = services.BuildServiceProvider();

            var clientTelemetry = serviceProvider.GetService<TelemetryClient>();

            var servicoTelemetria = new ServicoTelemetria(clientTelemetry, telemetriaOptions);

            DapperExtensionMethods.Init(servicoTelemetria);

            services.AddSingleton(servicoTelemetria);
        }
        private void ConfiguraVariaveisAmbiente(IServiceCollection services)
        {
            configuracaoRabbitOptions = new ConfiguracaoRabbitOptions();
            configuration.GetSection(nameof(ConfiguracaoRabbitOptions)).Bind(configuracaoRabbitOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SME.SGP.Fechamento.Worker v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitFechamento!");
            });
        }
    }
}
