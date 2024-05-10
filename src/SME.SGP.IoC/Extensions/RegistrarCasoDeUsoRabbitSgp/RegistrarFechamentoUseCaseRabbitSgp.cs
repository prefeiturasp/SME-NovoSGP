using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarFechamentoUseCaseRabbitSgp(this IServiceCollection services)
        {
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
            services.TryAddScoped<IExecutarConsolidacaoDreConselhoClasseUseCase, ExecutarConsolidacaoDreConselhoClasseUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoUeConselhoClasseUseCase, ExecutarConsolidacaoUeConselhoClasseUseCase>();
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
            services.TryAddScoped<INotificarAlteracaoNotaFechamentoAgrupadaUseCase, NotificarAlteracaoNotaFechamentoAgrupadaUseCase>();
            services.TryAddScoped<INotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase, NotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaGeralUseCase, ExecutarConsolidacaoTurmaGeralUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaUseCase, ExecutarConsolidacaoTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase, ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase>();
            services.TryAddScoped<IGerarFechamentoTurmaEdFisica2020UseCase, GerarFechamentoTurmaEdFisica2020UseCase>();
            services.TryAddScoped<IGerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase, GerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase>();
            services.TryAddScoped<INotificarAlteracaoNotaPosConselhoAgrupadaTurmaUseCase, NotificarAlteracaoNotaPosConselhoAgrupadaTurmaUseCase>();
            services.TryAddScoped<INotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase, NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase>();
        }
    }
}
