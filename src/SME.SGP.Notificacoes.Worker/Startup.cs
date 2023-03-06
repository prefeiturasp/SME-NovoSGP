using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.SGP.Infra;
using SME.SGP.IoC;

namespace SME.SGP.Notificacoes.Worker
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
            services.AddPolicies();
            services.ConfigurarRabbit(Configuration);
            services.ConfigurarTelemetria(Configuration);

            RegistrarHub(services);

            services.AddHostedService<WorkerRabbitNotificacao>();
            services.AddHealthChecks();
            services.AddHealthChecksUiSgp();            
        }

        private void RegistrarHub(IServiceCollection services)
        {
            services.AddOptions<HubOptions>()
                .Bind(Configuration.GetSection(HubOptions.Secao), c => c.BindNonPublicProperties = true);

            services.AddSingleton<HubOptions>();
            services.AddSingleton<INotificacaoSgpHub, NotificacaoSgpHub>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new HttpDiagnosticsSubscriber());
            
            app.UseHealthChecksSgp();
            app.UseHealthCheckPrometheusSgp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitNotificacoes!");
            });
        }
    }
}
