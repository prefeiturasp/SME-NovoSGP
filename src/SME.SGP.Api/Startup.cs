using Dapper;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using SME.SGP.Infra;
using SME.SGP.Api.Configuracoes;
using SME.SGP.IoC.Extensions;

namespace SME.SGP.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseRouting();
            app.UseAuthorization();

            app.UseSwagger();                                         
            app.UseSwaggerUI(c =>                                         
            {                                                             
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SGP Api");
            });

            //TODO: Ajustar para as os origins que irão consumir
            app.UseCors(config => config
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            RegistrarConfigsThreads.Registrar(Configuration);

            Console.WriteLine("CURRENT------", Directory.GetCurrentDirectory());
            Console.WriteLine("COMBINE------", Path.Combine(Directory.GetCurrentDirectory(), @"Imagens"));
            
            app.UseHealthChecksSgp();
            app.UseHealthCheckPrometheusSgp();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            var configTamanhoLimiteRequest = Configuration.GetSection("SGP_MaxRequestSizeBody").Value ?? "104857600";

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = long.Parse(configTamanhoLimiteRequest);
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddSingleton(Configuration);
            services.AddHttpContextAccessor();

            var registraDependencias = new RegistrarDependencias();
            registraDependencias.Registrar(services, Configuration);
            registraDependencias.RegistrarGoogleClassroomSync(services, Configuration);
            registraDependencias.RegistrarHttpClients(services, Configuration);
            registraDependencias.RegistrarPolicies(services);

            RegistraAutenticacao.Registrar(services, Configuration);
            RegistrarMvc.Registrar(services); 
            RegistraDocumentacaoSwagger.Registrar(services);

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddHealthChecks()
                .AddPostgreSqlSgp(Configuration)
                .AddRedisSgp()
                .AddRabbitMqSgp(Configuration)
                .AddRabbitMqLogSgp(Configuration);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
                //ta duplicado esse culture mesmo ?
                options.SupportedCultures = new List<CultureInfo> {new("pt-BR")};
            });
            
            services.AddHealthChecksUiSgp()
                .AddPostgreSqlStorageSgp(Configuration);
            
            services.AddCors();
            services.AddControllers();
        }
    }
}