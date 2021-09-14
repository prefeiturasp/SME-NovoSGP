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
            var assemblyApplication = AppDomain.CurrentDomain.Load("SME.SGP.Aplicacao");
            var assemblyDomain = AppDomain.CurrentDomain.Load("SME.SGP.Agendador.Dominio");
            services.AddMediatR(assemblyApplication, assemblyDomain);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));
        }
    }
}
