using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional.Frequencia;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional.Frequencia;

namespace SME.SGP.IoC.Extensions.RegistrarCasoDeUsoRabbitSgp
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarPainelEducacionalUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.AddScoped<IConsolidarIdepPainelEducacionalUseCase, ConsolidarIdepPainelEducacionalUseCase>();
            services.AddScoped<IConsolidarIdebPainelEducacionalUseCase, ConsolidarIdebPainelEducacionalUseCase>();
            services.AddScoped<IConsolidarVisaoGeralPainelEducacionalUseCase, ConsolidarVisaoGeralPainelEducacionalUseCase>();
            services
                .AddScoped<IConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase,
                           ConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase>()
                .AddScoped<IConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase,
                           ConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase>()
                .AddScoped<IConsolidarInformacoesPapPainelEducacionalUseCase,
                           ConsolidarInformacoesPapPainelEducacionalUseCase>()
               .AddScoped<IConsolidarFluenciaLeitoraPainelEducacionalUseCase,
                           ConsolidarFluenciaLeitoraPainelEducacionalUseCase>()
               .AddScoped<IConsolidarReclassificacaoPainelEducacionalUseCase,
                           ConsolidarReclassificacaoPainelEducacionalUseCase>();
        }
    }
}
