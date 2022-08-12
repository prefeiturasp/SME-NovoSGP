using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.IoC
{
    internal static class RegistrarAuditoria
    {
        internal static void ConfigurarAuditoria(this IServiceCollection services)
        {
            services.AddSingleton<IServicoAuditoria, ServicoAuditoria>();
        }
    }
}
