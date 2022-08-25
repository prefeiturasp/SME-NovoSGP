using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Notificacoes.Hub
{
    public class Startup
    {
        public const string CustomCookieScheme = nameof(CustomCookieScheme);
        public const string CustomTokenScheme = nameof(CustomTokenScheme);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegistrarSignalR(services);
            RegistrarEventosNotificacao(services);
            RegistrarCache(services);

            RegistrarAutenticacao(services);
        }

        private void RegistrarAutenticacao(IServiceCollection services)
        {
            services.AddAuthentication();

            services.AddAuthorization(c =>
            {
                c.AddPolicy("Token", pb => pb
                    .AddAuthenticationSchemes(CustomTokenScheme)
                    .RequireClaim("token")
                    .RequireAuthenticatedUser());
            });

        }

        private void RegistrarCache(IServiceCollection services)
        {
            services.AddOptions<RedisOptions>()
                .Bind(Configuration.GetSection(RedisOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<RedisOptions>();

            services.AddSingleton<IRepositorioCache>(serviceProvider =>
            {
                var redisOptions = serviceProvider.GetService<IOptions<RedisOptions>>()?.Value;
                var connection = new ConnectionMultiplexerSME(redisOptions);
                return new RepositorioCacheRedis(connection, redisOptions);

            });
            services.AddSingleton<IRepositorioUsuario>(serviceProvider =>
            {
                var repositorioCache = serviceProvider.GetService<IRepositorioCache>();
                var repositorioUsuario = new RepositorioUsuario(repositorioCache);

                EventoNotificacaoExtensions.Inicializa(repositorioUsuario);
                return repositorioUsuario;
            });
            //services.AddSingleton<IUserIdProvider, RedisUserIdProvider>();
        }

        private void RegistrarEventosNotificacao(IServiceCollection services)
        {
            services.TryAddScoped<IEventoNotificacaoCriada, EventoNotificacaoCriada>();
            services.TryAddScoped<IEventoNotificacaoLida, EventoNotificacaoLida>();
            services.TryAddScoped<IEventoNotificacaoExcluida, EventoNotificacaoExcluida>();
        }

        private void RegistrarSignalR(IServiceCollection services)
        {
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new HttpDiagnosticsSubscriber());

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificacaoHub>("/notificacao");
            });
        }
    }
}
