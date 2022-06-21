using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IReceberRelatorioProntoUseCase, ReceberRelatorioProntoUseCase>();
            services.TryAddScoped<IReceberRelatorioProntoEscolaAquiUseCase, ReceberRelatorioProntoEscolaAquiUseCase>();
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
            services.TryAddScoped<IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase, ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase>();
            services.TryAddScoped<INotificacaoReuniaoPedagogicaUseCase, NotificacaoReuniaoPedagogicaUseCase>();
            services.TryAddScoped<ITrataNotificacoesNiveisCargosUseCase, TrataNotificacoesNiveisCargosUseCase>();
            services.TryAddScoped<IGerarPendenciaAusenciaRegistroIndividualUseCase, GerarPendenciaAusenciaRegistroIndividualUseCase>();
            services.TryAddScoped<IAtualizarPendenciaRegistroIndividualUseCase, AtualizarPendenciaRegistroIndividualUseCase>();
            services.TryAddScoped<INotificarCompensacaoAusenciaUseCase, NotificarCompensacaoAusenciaUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaGeralUseCase, ExecutarConsolidacaoTurmaGeralUseCase>();
            services.TryAddScoped<IExecutarConsolidacaoTurmaUseCase, ExecutarConsolidacaoTurmaUseCase>();
            services.TryAddScoped<IConsolidarDevolutivasPorTurmaInfantilUseCase, ConsolidarDevolutivasPorTurmaInfantilUseCase>();
            services.TryAddScoped<IConsolidarDevolutivasPorTurmaUseCase, ConsolidarDevolutivasPorTurmaUseCase>();
            services.TryAddScoped<IConsolidarDiariosBordoCarregarUseCase, ConsolidarDiariosBordoCarregarUseCase>();
            services.TryAddScoped<IConsolidarDiariosBordoPorUeTratarUseCase, ConsolidarDiariosBordoPorUeTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosPorUeTratarUseCase, ConsolidarRegistrosPedagogicosPorUeTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosPorTurmaTratarUseCase, ConsolidarRegistrosPedagogicosPorTurmaTratarUseCase>();
            services.TryAddScoped<IRabbitDeadletterSgpSyncUseCase, RabbitDeadletterSgpSyncUseCase>(); // TODO [Fernando Groeler] verificar se essa fila é necessário registrar
            services.TryAddScoped<IRabbitDeadletterSgpTratarUseCase, RabbitDeadletterSgpTratarUseCase>();
            services.TryAddScoped<IRabbitDeadletterSrTratarUseCase, RabbitDeadletterSrTratarUseCase>(); // TODO [Fernando Groeler] verificar se essa fila é necessário registrar
            services.TryAddScoped<IConsolidacaoMediaRegistrosIndividuaisTurmaUseCase, ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase>();
            services.TryAddScoped<IConsolidacaoMediaRegistrosIndividuaisUseCase, ConsolidacaoMediaRegistrosIndividuaisUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase>();
            services.TryAddScoped<IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase, ConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase>();
            services.TryAddScoped<IConsolidarRegistrosPedagogicosUseCase, ConsolidarRegistrosPedagogicosUseCase>();
            services.TryAddScoped<IImportarAtividadesGsaUseCase, ImportarAtividadesGsaUseCase>();
            services.TryAddScoped<IImportarNotaAtividadeAvaliativaGsaUseCase, ImportarNotaAtividadeAvaliativaGsaUseCase>();
            services.TryAddScoped<IRotasAgendamentoTratarUseCase, RotasAgendamentoTratarUseCase>();
            services.TryAddScoped<IExcluirWorkflowAprovacaoPorIdUseCase, ExcluirWorkflowAprovacaoPorIdUseCase>();
            services.TryAddScoped<IExcluirDiarioBordoPorAulaIdUseCase, ExcluirDiarioBordoPorAulaIdUseCase>();
            services.TryAddScoped<IExecutarTipoCalendarioUseCase, ExecutarTipoCalendarioUseCase>();
            services.TryAddScoped<IExecutarGravarRecorrenciaUseCase, ExecutarGravarRecorrenciaUseCase>();
            services.TryAddScoped<IAlterarRecorrenciaEventosUseCase, AlterarRecorrenciaEventosUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoObjetivosComJuremaUseCase, ExecutarSincronizacaoObjetivosComJuremaUseCase>();
            services.TryAddScoped<IExecutarNotificacaoAlunosFaltososBimestreUseCase, ExecutarNotificacaoAlunosFaltososBimestreUseCase>();
            services.TryAddScoped<IExecutarTratamentoNotificacoesNiveisCargosUseCase, ExecutarTratamentoNotificacoesNiveisCargosUseCase>();
            services.TryAddScoped<IExecutarSincronismoComponentesCurricularesUseCase, ExecutarSincronismoComponentesCurricularesUseCase>();
            services.TryAddScoped<IExecutaSincronismoComponentesCurricularesEolUseCase, ExecutaSincronismoComponentesCurricularesEolUseCase>();
            services.TryAddScoped<IExecutarSyncGeralGoogleClassroomUseCase, ExecutarSyncGeralGoogleClassroomUseCase>();
            services.TryAddScoped<IExecutaSyncGsaGoogleClassroomUseCase, ExecutaSyncGsaGoogleClassroomUseCase>();
            services.TryAddScoped<IExecutarSyncSerapEstudantesProvasUseCase, ExecutarSyncSerapEstudantesProvasUseCase>();
            services.TryAddScoped<ICarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase, CarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase>();
            services.TryAddScoped<ISyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase, SyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase>();
            services.TryAddScoped<IAtualizarUltimoLoginUsuarioUseCase, AtualizarUltimoLoginUsuarioUseCase>();
        }
    }
}
