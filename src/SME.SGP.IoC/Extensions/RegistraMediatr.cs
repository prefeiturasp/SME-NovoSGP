using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Pipelines;
using System;

namespace SME.SGP.IoC
{
    public static class RegistraMediatr
    {
        public static void AdicionarMediatr(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("SME.SGP.Aplicacao");
            services.AddMediatR(assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));
        }
    }
}
