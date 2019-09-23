using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao.Configuracoes;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.IoC;
using Swashbuckle.AspNetCore.Swagger;
using System;
using static System.Text.Encoding;

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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

            app.UseMvc();
            app.UseMetricServer();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            RegistraDependencias.Registrar(services);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            RegistrarMapeamentos.Registrar();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddMvc(options =>
            {
                options.AllowValidatingTopLevelNodes = false;
                options.EnableEndpointRouting = true;
                options.Filters.Add(new ValidaDtoAttribute());
                options.Filters.Add(new FiltroExcecoesAttribute(Configuration));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SGP v1", Version = "v1" });
            });

            services.AddHttpContextAccessor();

            services.AddHttpClient<IServicoJurema, ServicoJurema>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetSection("UrlApiJurema").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("SGP-Redis");
                options.InstanceName = Configuration.GetValue<string>("Nome-Instancia-Redis");
            });

            services.AddHttpClient<IServicoEOL, ServicoEOL>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            var jwtConfiguration = Configuration
       .GetSection(nameof(JwtTokenSettings));
            services.Configure<JwtTokenSettings>(jwtConfiguration);
            var jwtTokenSettings = jwtConfiguration
                .Get<JwtTokenSettings>();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = jwtTokenSettings.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(UTF8
                        .GetBytes(jwtTokenSettings.IssuerSigningKey))
                };
                //o.Events = new JwtBearerEvents()
                //{
                //    OnAuthenticationFailed = c =>
                //    {
                //        if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                //        {
                //            c.NoResult();
                //            c.Response.Headers.Add("Token-Expired", "true");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
            });
        }
    }
}