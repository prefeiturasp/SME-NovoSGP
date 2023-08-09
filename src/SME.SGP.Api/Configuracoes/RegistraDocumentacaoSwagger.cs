using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Integracoes;

namespace SME.SGP.Api.Configuracoes
{
    public static class RegistraDocumentacaoSwagger
    {
        public static void Registrar(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var versaoService = serviceProvider.GetService<IServicoGithub>()!;
            var versaoAtual = versaoService.RecuperarUltimaVersao()
                .GetAwaiter().GetResult();

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

                c.OperationFilter<BasicAuthOperationsFilter>();
            });

            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<FiltroIntegracaoExterna>();
            });
        }
    }
}
