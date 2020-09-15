using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;

namespace SME.SGP.Api
{
    public static class RegistrarMvc
    {
        public static void Registrar(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = true;
                options.Filters.Add(new ValidaDtoAttribute());
                options.Filters.Add(new FiltroExcecoesAttribute(configuration));
            }).AddFluentValidation();
        }
    }
}