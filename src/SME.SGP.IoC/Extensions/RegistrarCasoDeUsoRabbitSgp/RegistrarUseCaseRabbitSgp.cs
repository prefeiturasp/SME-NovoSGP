using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.CasosDeUso.AbrangenciaSync;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AbrangenciaSync;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<INotificarUsuarioUseCase, NotificarUsuarioUseCase>();
            services.TryAddScoped<IReceberRelatorioProntoUseCase, ReceberRelatorioProntoUseCase>();
            services.TryAddScoped<IReceberRelatorioProntoEscolaAquiUseCase, ReceberRelatorioProntoEscolaAquiUseCase>();
            services.TryAddScoped<IReceberRelatorioComErroUseCase, ReceberRelatorioComErroUseCase>();
            services.TryAddScoped<ICopiarCodigoCorrelacaoUseCase, CopiarCodigoCorrelacaoUseCase>();
            services.TryAddScoped<IInserirCodigoCorrelacaoUseCase, InserirCodigoCorrelacaoUseCase>();
            services.TryAddScoped<INotificarDiarioBordoObservacaoUseCase, NotificarDiarioBordoObservacaoUseCase>();
            services.TryAddScoped<IAlterarNotificacaoObservacaoDiarioBordoUseCase, AlterarNotificacaoObservacaoDiarioBordoUseCase>();
            services.TryAddScoped<ISalvarNotificacaoCartaIntencoesObservacaoUseCase, SalvarNotificacaoCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<IExcluirNotificacaoCartaIntencoesObservacaoUseCase, ExcluirNotificacaoCartaIntencoesObservacaoUseCase>();
            services.TryAddScoped<ISalvarNotificacaoDevolutivaUseCase, SalvarNotificacaoDevolutivaUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDevolutivaUseCase, ExcluirNotificacaoDevolutivaUseCase>();
            services.TryAddScoped<IExcluirNotificacaoDiarioBordoUseCase, ExcluirNotificacaoDiarioBordoUseCase>();
            services.TryAddScoped<ISincronizarComponentesCurricularesUseCase, SincronizarComponentesCurricularesUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciaDiasLetivosInsuficientes, ExecutarExclusaoPendenciaDiasLetivosInsuficientes>();
            services.TryAddScoped<IExecutarExclusaoPendenciaParametroEvento, ExecutarExclusaoPendenciaParametroEvento>();
            services.TryAddScoped<INotificacaoReuniaoPedagogicaUseCase, NotificacaoReuniaoPedagogicaUseCase>();
            services.TryAddScoped<ITrataNotificacoesNiveisCargosUseCase, TrataNotificacoesNiveisCargosUseCase>();
            services.TryAddScoped<ITrataNotificacoesNiveisCargosDreUseCase, TrataNotificacoesNiveisCargosDreUseCase>();
            services.TryAddScoped<ITrataNotificacoesNiveisCargosUeUseCase, TrataNotificacoesNiveisCargosUeUseCase>();
            services.TryAddScoped<IGerarPendenciaAusenciaRegistroIndividualUseCase, GerarPendenciaAusenciaRegistroIndividualUseCase>();
            services.TryAddScoped<IAtualizarPendenciaRegistroIndividualUseCase, AtualizarPendenciaRegistroIndividualUseCase>();
            services.TryAddScoped<INotificarCompensacaoAusenciaUseCase, NotificarCompensacaoAusenciaUseCase>();
            services.TryAddScoped<IConsolidarDevolutivasPorUeUseCase, ConsolidarDevolutivasPorUeUseCase>();
            services.TryAddScoped<IConsolidarDevolutivasPorTurmaInfantilUseCase, ConsolidarDevolutivasPorTurmaInfantilUseCase>();
            services.TryAddScoped<IConsolidarDevolutivasPorTurmaUseCase, ConsolidarDevolutivasPorTurmaUseCase>();
            services.TryAddScoped<IConsolidarDiariosBordoCarregarUseCase, ConsolidarDiariosBordoCarregarUseCase>();
            services.TryAddScoped<IConsolidarDiariosBordoPorUeTratarUseCase, ConsolidarDiariosBordoPorUeTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosPorUeTratarUseCase, ConsolidarRegistrosPedagogicosPorUeTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosPorTurmaTratarUseCase, ConsolidarRegistrosPedagogicosPorTurmaTratarUseCase>();
            services.TryAddScoped<IConsolidacaoMediaRegistrosIndividuaisTurmaUseCase, ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoMediaRegistrosIndividuaisUseCase, ConsolidacaoMediaRegistrosIndividuaisUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosUseCase, ConsolidarRegistrosPedagogicosUseCase>();
            services.TryAddScoped<IRotasAgendamentoTratarUseCase, RotasAgendamentoTratarUseCase>();
            services.TryAddScoped<IExcluirWorkflowAprovacaoPorIdUseCase, ExcluirWorkflowAprovacaoPorIdUseCase>();
            services.TryAddScoped<IExcluirDiarioBordoPorAulaIdUseCase, ExcluirDiarioBordoPorAulaIdUseCase>();
            services.TryAddScoped<IExecutarTipoCalendarioUseCase, ExecutarTipoCalendarioUseCase>();
            services.TryAddScoped<IExecutarGravarRecorrenciaUseCase, ExecutarGravarRecorrenciaUseCase>();
            services.TryAddScoped<IAlterarRecorrenciaEventosUseCase, AlterarRecorrenciaEventosUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoObjetivosComJuremaUseCase, ExecutarSincronizacaoObjetivosComJuremaUseCase>();
            services.TryAddScoped<IExecutarNotificacaoAlunosFaltososBimestreUseCase, ExecutarNotificacaoAlunosFaltososBimestreUseCase>();
            services.TryAddScoped<IExecutarSincronismoComponentesCurricularesUseCase, ExecutarSincronismoComponentesCurricularesUseCase>();
            services.TryAddScoped<IExecutaSincronismoComponentesCurricularesEolUseCase, ExecutaSincronismoComponentesCurricularesEolUseCase>();
            services.TryAddScoped<IExecutarSyncGeralGoogleClassroomUseCase, ExecutarSyncGeralGoogleClassroomUseCase>();
            services.TryAddScoped<IExecutaSyncGsaGoogleClassroomUseCase, ExecutaSyncGsaGoogleClassroomUseCase>();
            services.TryAddScoped<IExecutarSyncSerapEstudantesProvasUseCase, ExecutarSyncSerapEstudantesProvasUseCase>();
            services.TryAddScoped<ICarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase, CarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase>();
            services.TryAddScoped<ISyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase, SyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase>();
            services.TryAddScoped<IAtualizarUltimoLoginUsuarioUseCase, AtualizarUltimoLoginUsuarioUseCase>();
            services.TryAddScoped<IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase, ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase>();
            services.TryAddScoped<IReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase, ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase>();
            services.TryAddScoped<IReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase, ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase>();
            services.TryAddScoped<IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase, ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoResponsaveisUseCase, RemoverAtribuicaoResponsaveisUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoResponsaveisSupervisorPorDreUseCase, RemoverAtribuicaoResponsaveisSupervisorPorDreUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoResponsaveisPAAIPorDreUseCase, RemoverAtribuicaoResponsaveisPAAIPorDreUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase, RemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            services.TryAddScoped<IExcluirArmazenamentoPorAquivoUseCase, ExcluirArmazenamentoPorAquivoUseCase>();
            services.TryAddScoped<IExecutarGravarHistoricoEscolarObservacaoUseCase, ExecutarGravarHistoricoEscolarObservacaoUseCase>();
            services.TryAddScoped<IAtualizarInformacoesDoPlanoAEEUseCase, AtualizarInformacoesDoPlanoAEEUseCase>();
            services.TryAddScoped<IAtualizarTurmaDoPlanoAEEUseCase, AtualizarTurmaDoPlanoAEEUseCase>();
            //Migração PAP
            services.TryAddScoped<IExecutarMigracaoRelatorioPAPPorIdUseCase, ExecutarMigracaoRelatorioPAPPorIdUseCase>();
            services.TryAddScoped<IExecutarMigracaoRelatorioPapPorAnoEletivoUseCase, ExecutarMigracaoRelatorioPapPorAnoEletivoUseCase>();
            services.TryAddScoped<IExecutarMigracaoRelatorioSemestralPAPUseCase, ExecutarMigracaoRelatorioSemestralPAPUseCase>();

            services.TryAddScoped<IExecutarNotificacaoInformativoUsuariosUseCase, ExecutarNotificacaoInformativoUsuariosUseCase>();
            services.TryAddScoped<IExecutarNotificacaoInformativoUsuarioUseCase, ExecutarNotificacaoInformativoUsuarioUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacaoInformativoUsuariosUseCase, ExecutarExclusaoNotificacaoInformativoUsuariosUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacaoInformativoUsuarioUseCase, ExecutarExclusaoNotificacaoInformativoUsuarioUseCase>();
            services.TryAddScoped<IGerarCacheAtribuicaoResponsaveisUseCase, GerarCacheAtribuicaoResponsaveisUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacoesPeriodicamenteUseCase, ExecutarExclusaoNotificacoesPeriodicamenteUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacaoUseCase, ExecutarExclusaoNotificacaoUseCase>();
            services.TryAddScoped<IAtualizarMapeamentoDosEstudantesUseCase, AtualizarMapeamentoDosEstudantesUseCase>();
            services.TryAddScoped<IAtualizarMapeamentoDoEstudanteDoBimestreUseCase, AtualizarMapeamentoDoEstudanteDoBimestreUseCase>();
            services.TryAddScoped<IAbrangenciaSyncUseCase, AbrangenciaSyncUseCase>();
        }
    }
}
