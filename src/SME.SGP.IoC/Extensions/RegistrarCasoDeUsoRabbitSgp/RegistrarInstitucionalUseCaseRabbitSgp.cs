using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarInstitucionalUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalDreSyncUseCase, ExecutarSincronizacaoInstitucionalDreSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalDreTratarUseCase, ExecutarSincronizacaoInstitucionalDreTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalUeTratarUseCase, ExecutarSincronizacaoInstitucionalUeTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase, ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase, ExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalCicloSyncUseCase, ExecutarSincronizacaoInstitucionalCicloSyncUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalCicloTratarUseCase, ExecutarSincronizacaoInstitucionalCicloTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTurmaSyncUseCase, ExecutarSincronizacaoInstitucionalTurmaSyncUseCase>();
            services.TryAddScoped<ICarregarDresConsolidacaoMatriculaUseCase, CarregarDresConsolidacaoMatriculaUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoDresConsolidacaoMatriculasUseCase, ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase>();
            services.TryAddScoped<ICarregarMatriculaTurmaUseCase, CarregarMatriculaTurmaUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase, ExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTurmaTratarUseCase, ExecutarSincronizacaoInstitucionalTurmaTratarUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase, ExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase>();

            #region ATENÇÃO - Use Cases injetados em processos consumidos pelo o worker
            // TODO
            services.TryAddScoped<IObterDataCriacaoRelatorioUseCase, ObterDataCriacaoRelatorioUseCase>();
            services.TryAddScoped<IRepositorioTipoRelatorio, RepositorioTipoRelatorio>();

            #endregion
        }
    }
}
