using Microsoft.OpenApi.Models;
using SME.SGP.Api.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace SME.SGP.Api.Filtros
{
	public class FiltroIntegracaoExterna : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                    .Union(context.MethodInfo.GetCustomAttributes(true))
                                    .OfType<ChaveIntegracaoSgpApi>();

            if (attributes != null && attributes.Any())
            {

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "x-sgp-api-key",
                    In = ParameterLocation.Header,
                    Required = false
                });
            }
        }
    }
}
