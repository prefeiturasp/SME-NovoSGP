using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Integracoes;

namespace SME.SGP.Api
{
	public static class RegistraDocumentacaoSwagger
    {
        public static void Registrar(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            var versaoService = sp.GetService<IServicoGithub>();
            var versaoAtual = versaoService.RecuperarUltimaVersao().Result;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"SGP v1",
                    Version = versaoAtual
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Para autenticação, incluir 'Bearer' seguido do token JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<FiltroIntegracaoExterna>();
            });

        }
    }
}
