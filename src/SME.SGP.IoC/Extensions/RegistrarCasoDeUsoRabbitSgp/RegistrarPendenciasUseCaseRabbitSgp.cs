using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarPendenciasUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisUseCase, ExecutaVerificacaoPendenciasGeraisUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisCalendarioUseCase, ExecutaVerificacaoPendenciasGeraisCalendarioUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisEventosUseCase, ExecutaVerificacaoPendenciasGeraisEventosUseCase>();
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase, ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase>();
            services.TryAddScoped<ITratarAtribuicaoPendenciasUsuariosUseCase, TratarAtribuicaoPendenciasUsuariosUseCase>();
            services.TryAddScoped<ICargaAtribuicaoPendenciasPerfilUsuarioUseCase, CargaAtribuicaoPendenciasPerfilUsuarioUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUseCase, RemoverAtribuicaoPendenciasUsuariosUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUeUseCase, RemoverAtribuicaoPendenciasUsuariosUeUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase, RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasDevolutivaUseCase, ExecutarExclusaoPendenciasDevolutivaUseCase>();
            services.TryAddScoped<IReplicarParametrosAnoAnteriorUseCase, ReplicarParametrosAnoAnteriorUseCase>();
            services.TryAddScoped<IExcluirPendenciaCalendarioAnoAnteriorCalendarioUseCase, ExcluirPendenciaCalendarioAnoAnteriorUseCase>();
            services.TryAddScoped<IExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase, ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase>();
            services.TryAddScoped<IRemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase, RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase>();
            services.TryAddScoped<IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase, RemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase>();
            services.TryAddScoped<IRemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase, RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase>();
            services.TryAddScoped<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase, RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            services.TryAddScoped<IRemoverPendenciasNoFinalDoAnoLetivoUseCase, RemoverPendenciasNoFinalDoAnoLetivoUseCase>();
            services.TryAddScoped<IObterQuantidadeAulaDiaPendenciaUseCase, ObterQuantidadeAulaDiaPendenciaUseCase>();
            services.TryAddScoped<ICargaQuantidadeAulaDiaPendenciaUseCase, CargaQuantidadeAulaDiaPendenciaUseCase>();
            services.TryAddScoped<IObterQuantidadeAulaDiaPendenciaPorUeUseCase, ObterQuantidadeAulaDiaPendenciaPorUeUseCase>();
        }
    }
}
