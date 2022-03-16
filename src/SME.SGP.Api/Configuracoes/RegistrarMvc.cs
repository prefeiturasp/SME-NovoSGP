using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Infra;

namespace SME.SGP.Api
{
    public static class RegistrarMvc
    {
        public static void Registrar(IServiceCollection services, ServiceProvider serviceProvider)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
            var conexao = serviceProvider.GetService<ISgpContext>();
            var mediator = serviceProvider.GetService<IMediator>();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = true;
                options.Filters.Add(new ValidaDtoAttribute());
                options.Filters.Add(new FiltroExcecoesAttribute(mediator));
                options.Filters.Add(new DisposeConnectionFilter(conexao));
            })
                .AddFluentValidation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}