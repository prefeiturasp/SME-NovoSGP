using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Api.Filtros;
using SME.SGP.Api.Middlewares;
using SME.SGP.Infra;
using System.Text.Json.Serialization;

namespace SME.SGP.Api.Configuracoes
{
    public static class RegistrarMvc
    {
        public static void Registrar(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

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
                .AddNewtonsoftJson()
                .AddFluentValidation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
    }
}