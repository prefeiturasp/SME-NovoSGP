using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace SME.SGP.Api.Filtros
{
    public class AdicionaCabecalhoHttp : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "x-sgp-api-key",
                In = "header",
                Type = "string",
                Required = false
            });
        }
    }
}
