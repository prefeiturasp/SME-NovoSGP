using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;

namespace SME.SGP.IoC.Extensions.RegistrarCasoDeUsoRabbitSgp
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarPainelEducacionalUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.AddScoped<Aplicacao.Interfaces.CasosDeUso.PainelEducacional.IConsolidarIdepPainelEducacionalUseCase, Aplicacao.CasosDeUso.PainelEducacional.ConsolidarIdepPainelEducacionalUseCase>();
            services.AddScoped<IConsolidarIdebPainelEducacionalUseCase, ConsolidarIdebPainelEducacionalUseCase>();
            services.AddScoped<Aplicacao.Interfaces.CasosDeUso.PainelEducacional.IConsolidarVisaoGeralPainelEducacionalUseCase, Aplicacao.CasosDeUso.PainelEducacional.ConsolidarVisaoGeralPainelEducacionalUseCase>();
            services
                .AddScoped<IConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase, 
                           ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase>()
                .AddScoped<IConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase,
                           ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase>();
        }
    }
}
