using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.IoC
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarFechamentoUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<INotificarUsuarioUseCase, NotificarUsuarioUseCase>();
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase, ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaFechamentoUseCase, ExecutarExclusaoPendenciasAusenciaFechamentoUseCase>();
            services.TryAddScoped<IVerificaPendenciasFechamentoUseCase, VerificaPendenciasFechamentoUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoPorDreUseCase, ReprocessarParecerConclusivoPorDreUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoPorUeUseCase, ReprocessarParecerConclusivoPorUeUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoPorTurmaUseCase, ReprocessarParecerConclusivoPorTurmaUseCase>();
            services.TryAddScoped<IReprocessarParecerConclusivoAlunoUseCase, ReprocessarParecerConclusivoAlunoUseCase>();
            services.TryAddScoped<INotificarResultadoInsatisfatorioUseCase, NotificarResultadoInsatisfatorioUseCase>();
            services.TryAddScoped<IAtualizarSituacaoConselhoClasseUseCase, AtualizarSituacaoConselhoClasseUseCase>();
            services.TryAddScoped<INotificacaoAndamentoFechamentoUseCase, NotificacaoAndamentoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoAndamentoFechamentoPorUeUseCase, NotificacaoAndamentoFechamentoPorUeUseCase>();
            services.TryAddScoped<INotificacaoUeFechamentosInsuficientesUseCase, NotificacaoUeFechamentosInsuficientesUseCase>();
            services.TryAddScoped<IGerarPendenciasFechamentoUseCase, GerarPendenciasFechamentoUseCase>();
            services.TryAddScoped<INotificacaoInicioFimPeriodoFechamentoUseCase, NotificacaoInicioFimPeriodoFechamentoUseCase>();
            services.TryAddScoped<INotificacaoInicioPeriodoFechamentoUEUseCase, NotificacaoInicioPeriodoFechamentoUEUseCase>();
            services.TryAddScoped<INotificacaoFimPeriodoFechamentoUEUseCase, NotificacaoFimPeriodoFechamentoUEUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaConselhoClasseUseCase, ExecutarConsolidacaoTurmaConselhoClasseUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaFechamentoUseCase, ExecutarConsolidacaoTurmaFechamentoUseCase>();
            services.TryAddScoped<INotificarFechamentoReaberturaUseCase, NotificarFechamentoReaberturaUseCase>();
            services.TryAddScoped<INotificarFechamentoReaberturaDREUseCase, NotificarFechamentoReaberturaDREUseCase>();
            services.TryAddScoped<INotificarFechamentoReaberturaUEUseCase, NotificarFechamentoReaberturaUEUseCase>();
            services.TryAddScoped<IVarreduraFechamentosEmProcessamentoPendentesUseCase, VarreduraFechamentosEmProcessamentoPendentesUseCase>();
            services.TryAddScoped<IGerarNotificacaoAlteracaoLimiteDiasUseCase, GerarNotificacaoAlteracaoLimiteDiasUseCase>();
            services.TryAddScoped<IVerificarPendenciasFechamentoTurmaDisciplinaUseCase, VerificarPendenciasFechamentoTurmaDisciplinaUseCase>();
            services.TryAddScoped<IAlterarPeriodosComHierarquiaInferiorFechamentoUseCase, AlterarPeriodosComHierarquiaInferiorFechamentoUseCase>();
            services.TryAddScoped<INotificacaoPeriodoFechamentoReaberturaIniciandoUseCase, NotificacaoPeriodoFechamentoReaberturaIniciandoUseCase>();
            services.TryAddScoped<INotificacaoPeriodoFechamentoReaberturaEncerrandoUseCase, NotificacaoPeriodoFechamentoReaberturaEncerrandoUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase, ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaFechamentoComponenteUseCase, ExecutarConsolidacaoTurmaFechamentoComponenteUseCase>();

            #region ATENÇÃO - Use Cases injetados em processos consumidos pelo o worker
            // TODO
            services.TryAddScoped<IObterDataCriacaoRelatorioUseCase, ObterDataCriacaoRelatorioUseCase>();
            services.TryAddScoped<IRepositorioTipoRelatorio, RepositorioTipoRelatorio>();

            #endregion
        }
    }
}
