using Microsoft.Extensions.DependencyInjection;

namespace SME.SGP.IoC.Extensions.RegistrarCasoDeUsoRabbitSgp
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarPainelEducacionalUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.AddScoped<Aplicacao.Interfaces.CasosDeUso.PainelEducacional.IConsolidarIdepPainelEducacionalUseCase, Aplicacao.CasosDeUso.PainelEducacional.ConsolidarIdepPainelEducacionalUseCase>();
        }
    }
}
