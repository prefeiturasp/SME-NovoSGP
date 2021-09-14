using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Integracoes;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;

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
                c.SwaggerDoc("v1", new Info { Title = $"SGP v1", Version = versaoAtual });
                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Para autenticação, incluir 'Bearer' seguido do token JWT",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                            });
            });

            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<FiltroIntegracaoExterna>();
            });

        }
    }
}
