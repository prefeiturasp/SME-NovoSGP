﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarFrequenciaUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<ICalculoFrequenciaTurmaDisciplinaUseCase, CalculoFrequenciaTurmaDisciplinaUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaAlunoPorTurmaEMesUseCase, ConsolidarFrequenciaAlunoPorTurmaEMesUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmaEvasaoUseCase, ConsolidarFrequenciaTurmaEvasaoUseCase>();
            services.TryAddScoped<INotificacaoFrequenciaUeUseCase, NotificacaoFrequenciaUeUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasUseCase, ConsolidarFrequenciaTurmasUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorAnoUseCase, ConsolidarFrequenciaTurmasPorAnoUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorDREUseCase, ConsolidarFrequenciaTurmasPorDREUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaAnoSyncUseCase, ConciliacaoFrequenciaAnoSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmaDreSyncUseCase, ConciliacaoFrequenciaTurmaDreSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmaUeSyncUseCase, ConciliacaoFrequenciaTurmaUeSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasSyncUseCase, ConciliacaoFrequenciaTurmasSyncUseCase>();
            services.TryAddScoped<ICalcularFrequenciaGeralUseCase, CalcularFrequenciaGeralUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosSyncUseCase, ConciliacaoFrequenciaTurmasAlunosSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasPorPeriodoUseCase, ConciliacaoFrequenciaTurmasPorPeriodoUseCase>();
            services.TryAddScoped<IValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase, ValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosBuscarUseCase, ConciliacaoFrequenciaTurmasAlunosBuscarUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasMesUseCase, ConciliacaoFrequenciaTurmasMesUseCase>();
            services.TryAddScoped<ICarregarRegistroFrequenciaAlunosUseCase, CarregarRegistroFrequenciaAlunosUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase, ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase>();
            services.TryAddScoped<IExcluirFrequenciaPorAulaIdUseCase, ExcluirFrequenciaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirAnotacoesFrequenciaPorAulaIdUseCase, ExcluirAnotacoesFrequenciaPorAulaIdUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase, ExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoMensalDashBoardFrequenciaUseCase, ExecutaConsolidacaoMensalDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase, ExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase>();
            services.TryAddScoped<INotificacaoFrequenciaUseCase, NotificacaoFrequenciaUseCase>();
            services.TryAddScoped<IExecutarNotificacaoRegistroFrequenciaUseCase, ExecutarNotificacaoRegistroFrequenciaUseCase>();
            services.TryAddScoped<IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase, ConsolidacaoDashBoardFrequenciaPorDataETipoUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorUEUseCase, ConsolidarFrequenciaTurmasPorUEUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaPorTurmaUseCase, ConsolidarFrequenciaPorTurmaUseCase>();
        }
    }
}
