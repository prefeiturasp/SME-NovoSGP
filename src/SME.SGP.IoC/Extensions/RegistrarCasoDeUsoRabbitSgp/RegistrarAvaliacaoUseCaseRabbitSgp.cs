using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarAvaliacaoUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase, ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase>();
            services.TryAddScoped<IImportarAtividadesGsaUseCase, ImportarAtividadesGsaUseCase>();
            services.TryAddScoped<IImportarNotaAtividadeAvaliativaGsaUseCase, ImportarNotaAtividadeAvaliativaGsaUseCase>();
            services.TryAddScoped<IValidarMediaAlunosUseCase, ValidarMediaAlunosUseCase>();
            services.TryAddScoped<IValidarMediaAlunosAtividadeAvaliativaUseCase, ValidarMediaAlunosAtividadeAvaliativaUseCase>();
            services.TryAddScoped<INotificarUsuarioAlteracaoExtemporaneaUseCase, NotificarUsuarioAlteracaoExtemporaneaUseCase>();
        }
    }
}
