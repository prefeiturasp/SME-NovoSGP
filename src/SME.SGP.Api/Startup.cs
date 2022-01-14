using Dapper;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Prometheus;
using RabbitMQ.Client;
using SME.SGP.Api.HealthCheck;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace SME.SGP.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;

        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _env;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseElasticApm(Configuration, 
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SGP Api");
            });

            //TODO: Ajustar para as os origins que irão consumir
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMetricServer();

            app.UseHttpMetrics();

            app.UseAuthentication();

            app.UseMvc();

            app.UseStaticFiles();

            Console.WriteLine("CURRENT------", Directory.GetCurrentDirectory());
            Console.WriteLine("COMBINE------", Path.Combine(Directory.GetCurrentDirectory(), @"Imagens"));

            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });         

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            var configTamanhoLimiteRequest = Configuration.GetSection("SGP_MaxRequestSizeBody").Value ?? "104857600";
            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = long.Parse(configTamanhoLimiteRequest);
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddSingleton(Configuration);
            services.AddHttpContextAccessor();
            services.AddApplicationInsightsTelemetry(Configuration);

            ConfiguraVariaveisAmbiente(services);
            ConfiguraGoogleClassroomSync(services);
            ConfiguraRabbitParaLogs(services);
            var telemetriaOptions = ConfiguraTelemetria(services);

            RegistraDependencias.Registrar(services);

            var serviceProvider = services.BuildServiceProvider();

            var clientTelemetry = serviceProvider.GetService<TelemetryClient>();

            var servicoTelemetria = new ServicoTelemetria(clientTelemetry, telemetriaOptions);

            RegistraClientesHttp.Registrar(services, Configuration);
            RegistraAutenticacao.Registrar(services, Configuration);
            RegistrarMvc.Registrar(services, serviceProvider);
            RegistraDocumentacaoSwagger.Registrar(services);
            services.AddPolicies();

            DefaultTypeMap.MatchNamesWithUnderscores = true;            

            services.AddHealthChecks()
                    .AddNpgSql(
                        Configuration.GetConnectionString("SGP_Postgres"),
                        name: "Postgres")
                    .AddCheck<ApiJuremaCheck>("API Jurema")
                    .AddCheck<ApiEolCheck>("API EOL");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR"), new CultureInfo("pt-BR") };
            });            

            DapperExtensionMethods.Init(servicoTelemetria);

            services.AddSingleton(servicoTelemetria);

            services.AddMemoryCache();
        }

        private void ConfiguraVariaveisAmbiente(IServiceCollection services)
        {
            var configuracaoRabbitOptions = new ConfiguracaoRabbitOptions();
            Configuration.GetSection(nameof(ConfiguracaoRabbitOptions)).Bind(configuracaoRabbitOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitOptions);
        }

        private void ConfiguraGoogleClassroomSync(IServiceCollection services)
        {
            var googleClassroomSyncOptions = new GoogleClassroomSyncOptions();
            Configuration.GetSection(nameof(GoogleClassroomSyncOptions)).Bind(googleClassroomSyncOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(googleClassroomSyncOptions);
        }

        private void ConfiguraRabbitParaLogs(IServiceCollection services)
        {
            var configuracaoRabbitLogOptions = new ConfiguracaoRabbitLogOptions();
            Configuration.GetSection("ConfiguracaoRabbitLog").Bind(configuracaoRabbitLogOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(configuracaoRabbitLogOptions);
        }
        private TelemetriaOptions ConfiguraTelemetria(IServiceCollection services)
        {
            var telemetriaOptions = new TelemetriaOptions();
            Configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);

            services.AddSingleton(telemetriaOptions);

            return telemetriaOptions;
        }
    }
}
