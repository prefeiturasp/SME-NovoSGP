using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            services.AddAuthentication()
                .AddJwtBearer(CustomTokenScheme, o =>
                {
                    o.Events = new()
                    {
                        OnMessageReceived = (context) =>
                        {
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/login"))
                            {
                                var usuarioRf = context.Request.Query["usuarioRf"];

                                if (!string.IsNullOrWhiteSpace(usuarioRf))
                                {
                                    var claims = new Claim[]
                                    {
                                    new("usuario_rf", usuarioRf),
                                    new("token", "token_claim"),
                                    };
                                    var identity = new ClaimsIdentity(claims, CustomTokenScheme);
                                    context.Principal = new(identity);
                                    context.Success();
                                }

                            }
                            return Task.CompletedTask;
                        },
                    };
                });

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
                endpoints.Map("/user", ctx =>
                {
                    ctx.Response.StatusCode = 200;
                    return ctx.Response.WriteAsync(ctx.User?.Claims.FirstOrDefault(x => x.Type == "usuario_rf")?.Value);
                }).RequireAuthorization("Token");
                endpoints.Map("/login", ctx =>
                {
                    ctx.Response.StatusCode = 200;
                    return ctx.Response.WriteAsync(ctx.User?.Claims.FirstOrDefault(x => x.Type == "usuario_rf")?.Value);
                }).RequireAuthorization("Token");
            });
        }
    }
}
