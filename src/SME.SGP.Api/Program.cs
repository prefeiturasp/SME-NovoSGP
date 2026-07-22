using Dapper;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using SME.SGP.Api.Configuracoes;
using SME.SGP.Infra;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using Microsoft.AspNetCore.Localization;

namespace SME.SGP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            builder.Services.AddResponseCompression();

            var configTamanhoLimiteRequest = builder.Configuration.GetSection("SGP_MaxRequestSizeBody").Value ?? "104857600";

            builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = long.Parse(configTamanhoLimiteRequest);
            });

            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            builder.Services.AddSingleton(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            var registraDependencias = new RegistrarDependencias();
            registraDependencias.Registrar(builder.Services, builder.Configuration);
            registraDependencias.RegistrarGoogleClassroomSync(builder.Services, builder.Configuration);
            registraDependencias.RegistrarHttpClients(builder.Services, builder.Configuration);
            registraDependencias.RegistrarPolicies(builder.Services);

            RegistraAutenticacao.Registrar(builder.Services, builder.Configuration);
            RegistrarMvc.Registrar(builder.Services);
            RegistraDocumentacaoSwagger.Registrar(builder.Services);

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            builder.Services.AddHealthChecks()
                .AddPostgreSqlSgp(builder.Configuration)
                .AddRedisSgp()
                .AddRabbitMqSgp(builder.Configuration)
                .AddRabbitMqLogSgp(builder.Configuration);

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo> { new("pt-BR") };
            });

            builder.Services.AddHealthChecksUiSgp()
                .AddPostgreSqlStorageSgp(builder.Configuration);

            builder.Services.AddCors();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseElasticApm(app.Configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            app.UseResponseCompression();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
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

            app.UseCors(config => config
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            RegistrarConfigsThreads.Registrar(app.Configuration);

            app.UseHealthChecksSgp();
            app.UseHealthCheckPrometheusSgp();

            app.Run();
        }
    }
}