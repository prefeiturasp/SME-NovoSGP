using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AbrangenciaSync;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System.Linq;

namespace SME.SGP.Worker.RabbitMQ
{
    public class WorkerRabbitMQ : WorkerRabbitAplicacao
    {
        private const int TENTATIVA_REPROCESSAR_10 = 4;

        public WorkerRabbitMQ(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitMQ",
                typeof(RotasRabbitSgp))
        {
        }

        private void RegistrarUseCasesRelatorioComErro()
        {
            foreach (var rota in typeof(RotasRabbitSgp).ObterConstantesPublicas<string>().Where(a => a.Contains("sgp.relatorios.erro")))
                Comandos.Add(rota, new ComandoRabbit("Notificar relatório com erro", typeof(IReceberRelatorioComErroUseCase)));
        }

        private void RegistrarUseCasesRelatorios()
        {
            Comandos.Add(RotasRabbitSgp.RotaRelatoriosProntos, new ComandoRabbit("Receber dados do relatório", typeof(IReceberRelatorioProntoUseCase), TENTATIVA_REPROCESSAR_10, ExchangeSgpRabbit.SgpDeadLetterTTL_3));
            Comandos.Add(RotasRabbitSgp.RotaRelatoriosProntosApp, new ComandoRabbit("Receber dados do relatório do Escola Aqui", typeof(IReceberRelatorioProntoEscolaAquiUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRelatorioCorrelacaoCopiar, new ComandoRabbit("Copiar e gerar novo código de correlação", typeof(ICopiarCodigoCorrelacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRelatorioCorrelacaoInserir, new ComandoRabbit("Inserir novo código de correlação", typeof(IInserirCodigoCorrelacaoUseCase)));

            RegistrarUseCasesRelatorioComErro();
        }

        protected override void RegistrarUseCases()
        {
            RegistrarUseCasesRelatorios();

            Comandos.Add(RotasRabbitSgp.RotaNotificacaoUsuario, new ComandoRabbit("Notificar usuário", typeof(INotificarUsuarioUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo, new ComandoRabbit("Notificar usuário sobre nova observação no diário de bordo", typeof(INotificarDiarioBordoObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoAlterarObservacaoDiarioBordo, new ComandoRabbit("Alterar as notificações dos usuários excluídos das observação no diário de bordo", typeof(IAlterarNotificacaoObservacaoDiarioBordoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNovaNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Notificar usuário sobre nova observação na carta de intenções", typeof(ISalvarNotificacaoCartaIntencoesObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Excluir uma notificação sobre observação na carta de intenções", typeof(IExcluirNotificacaoCartaIntencoesObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNovaNotificacaoDevolutiva, new ComandoRabbit("Notificar usuário sobre a criação de uma devolutiva", typeof(ISalvarNotificacaoDevolutivaUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoDevolutiva, new ComandoRabbit("Excluir uma notificação sobre devolutiva", typeof(IExcluirNotificacaoDevolutivaUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoDiarioBordo, new ComandoRabbit("Excluir uma notificação do diario de bordo", typeof(IExcluirNotificacaoDiarioBordoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaSincronizaComponetesCurricularesEol, new ComandoRabbit("Sincroniza os compoentes curriculares com o Eol", typeof(ISincronizarComponentesCurricularesUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes, new ComandoRabbit("Executa exclusão de pendências de dias letivos insuficientes", typeof(IExecutarExclusaoPendenciaDiasLetivosInsuficientes)));
            Comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciaParametroEvento, new ComandoRabbit("Executa exclusão de pendências de eventos por parâmetro", typeof(IExecutarExclusaoPendenciaParametroEvento)));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoReuniaoPedagogica, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoReuniaoPedagogicaUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaTrataNotificacoesNiveis, new ComandoRabbit("Trata Níveis e Cargos das notificações aguardando ação", typeof(ITrataNotificacoesNiveisCargosUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Gerar as pendências por ausência de registro individual", typeof(IGerarPendenciaAusenciaRegistroIndividualUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaAtualizarPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Atualizar pendência por ausência de registro individual", typeof(IAtualizarPendenciaRegistroIndividualUseCase)));
            Comandos.Add(RotasRabbitSgp.NotificarCompensacaoAusencia, new ComandoRabbit("Notificar Compensação Ausência", typeof(INotificarCompensacaoAusenciaUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorUE, new ComandoRabbit("Consolidar Devolutivas UE", typeof(IConsolidarDevolutivasPorUeUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantil, new ComandoRabbit("Consolidar Devolutivas Turmas da Modalidade Infantil", typeof(IConsolidarDevolutivasPorTurmaInfantilUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new ComandoRabbit("Consolidar Devolutivas Turmas ", typeof(IConsolidarDevolutivasPorTurmaUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarDiariosBordoCarregar, new ComandoRabbit("Carregar UEs para consolidação de Diarios de Bordo", typeof(IConsolidarDiariosBordoCarregarUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarDiariosBordoPorUeTratar, new ComandoRabbit("Tratar consolidação de diarios de bordo por UE", typeof(IConsolidarDiariosBordoPorUeTratarUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorUeTratar, new ComandoRabbit("Tratar consolidação de registros pedagógicos por UE", typeof(IConsolidarRegistrosPedagogicosPorUeTratarUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorTurmaTratar, new ComandoRabbit("Tratar consolidação de registros pedagógicos por turma", typeof(IConsolidarRegistrosPedagogicosPorTurmaTratarUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarMediaRegistrosIndividuaisTurma, new ComandoRabbit("Consolidar Média de Registros Individuais", typeof(IConsolidacaoMediaRegistrosIndividuaisTurmaUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarMediaRegistrosIndividuais, new ComandoRabbit("Consolidar Média de Registros Individuais", typeof(IConsolidacaoMediaRegistrosIndividuaisUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAluno, new ComandoRabbit("Consolidar Acompanhamento Aprendizagem", typeof(IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoPorUE, new ComandoRabbit("Consolidar Acompanhamento Aprendizagem por UE", typeof(IConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar, new ComandoRabbit("Tratar Acompanhamento Aprendizagem", typeof(IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase), true));
            Comandos.Add(RotasRabbitSgp.ConsolidarRegistrosPedagogicos, new ComandoRabbit("Consolidar Registros Pedagógicos", typeof(IConsolidarRegistrosPedagogicosUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaAgendamentoTratar, new ComandoRabbit("Tratar mensagens de processamento periodico", typeof(IRotasAgendamentoTratarUseCase), true));
            Comandos.Add(RotasRabbitSgp.WorkflowAprovacaoExcluir, new ComandoRabbit("Executar exclusão de workflow de aprovação por id", typeof(IExcluirWorkflowAprovacaoPorIdUseCase)));
            Comandos.Add(RotasRabbitSgp.DiarioBordoDaAulaExcluir, new ComandoRabbit("Executar exclusão de diario de bordo por aula id", typeof(IExcluirDiarioBordoPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgp.ExecutarTipoCalendario, new ComandoRabbit("Executar o tipo calendario", typeof(IExecutarTipoCalendarioUseCase)));
            Comandos.Add(RotasRabbitSgp.ExecutarGravarRecorrencia, new ComandoRabbit("Executar a gravação das recorrencias", typeof(IExecutarGravarRecorrenciaUseCase)));
            Comandos.Add(RotasRabbitSgp.AlterarRecorrenciaEventos, new ComandoRabbit("Altera recorrencia de eventos", typeof(IAlterarRecorrenciaEventosUseCase)));
            Comandos.Add(RotasRabbitSgp.SincronizarObjetivosComJurema, new ComandoRabbit("Realiza sincronização dos objetivos com Jurema", typeof(IExecutarSincronizacaoObjetivosComJuremaUseCase)));
            Comandos.Add(RotasRabbitSgp.NotificarAlunosFaltososBimestre, new ComandoRabbit("Notifica os alunos faltoso no bimestre", typeof(IExecutarNotificacaoAlunosFaltososBimestreUseCase), true));
            Comandos.Add(RotasRabbitSgp.SincronizarComponentesCurriculares, new ComandoRabbit("Sicronizar componentes curriculares", typeof(IExecutarSincronismoComponentesCurricularesUseCase), true));
            Comandos.Add(RotasRabbitSgp.SincronizarComponentesCurricularesEol, new ComandoRabbit("Sicronizar componentes curriculares eol", typeof(IExecutaSincronismoComponentesCurricularesEolUseCase), true));
            Comandos.Add(RotasRabbitSgp.SyncGeralGoogleClassroom, new ComandoRabbit("Sicronizar geral google classroom", typeof(IExecutarSyncGeralGoogleClassroomUseCase), true));
            Comandos.Add(RotasRabbitSgp.SyncGsaGoogleClassroom, new ComandoRabbit("Sicronizar gsa google classroom", typeof(IExecutaSyncGsaGoogleClassroomUseCase), true));
            Comandos.Add(RotasRabbitSgp.SyncSerapEstudantesProvas, new ComandoRabbit("Sicronizar provas dos estantes serap ", typeof(IExecutarSyncSerapEstudantesProvasUseCase), true));
            Comandos.Add(RotasRabbitSgp.TratarNotificacoesNiveisCargos, new ComandoRabbit("Tratar notificações", typeof(ITrataNotificacoesNiveisCargosUseCase), true));
            Comandos.Add(RotasRabbitSgp.TratarNotificacoesNiveisCargosDre, new ComandoRabbit("Tratar notificações por dre", typeof(ITrataNotificacoesNiveisCargosDreUseCase), true));
            Comandos.Add(RotasRabbitSgp.TratarNotificacoesNiveisCargosUe, new ComandoRabbit("Tratar notificações por ue", typeof(ITrataNotificacoesNiveisCargosUeUseCase), true));
            Comandos.Add(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoCarregar, new ComandoRabbit("Efetua a atualização da rota das imagens do acompanhamento aluno", typeof(ICarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase)));
            Comandos.Add(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoSync, new ComandoRabbit("Efetua a atualização da rota das imagens do acompanhamento aluno", typeof(ISyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase)));
            Comandos.Add(RotasRabbitSgp.AtualizaUltimoLoginUsuario, new ComandoRabbit("Efetua a atualização da data de ultimo login do usuario", typeof(IAtualizarUltimoLoginUsuarioUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorDre, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por DRE", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorUe, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por UE", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorTurma, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por Turma", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorComponente, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por Componente", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase), true));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveis, new ComandoRabbit("Remover Atribuição de Responsaveis por DRE", typeof(IRemoverAtribuicaoResponsaveisUseCase), true));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveisSupervisorPorDre, new ComandoRabbit("Remover Atribuição de Responsaveis - Supervisor por DRE", typeof(IRemoverAtribuicaoResponsaveisSupervisorPorDreUseCase), true));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveisPAAIPorDre, new ComandoRabbit("Remover Atribuição de Responsaveis - PAAI por DRE", typeof(IRemoverAtribuicaoResponsaveisPAAIPorDreUseCase), true));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveisASPPorDre, new ComandoRabbit("Remover Atribuição de Responsaveis - Assistente Social, Psicólogo ou Psicopedagogo por DRE", typeof(IRemoverAtribuicaoResponsaveisASPPPorDreUseCase), true));
            Comandos.Add(RotasRabbitSgp.RemoverArquivoArmazenamento, new ComandoRabbit("Excluir Arquivo Armazenado no Minio", typeof(IExcluirArmazenamentoPorAquivoUseCase)));
            Comandos.Add(RotasRabbitSgp.ExecutarGravarObservacaoHistorioEscolar, new ComandoRabbit("Executar a gravação das observações complementares histórico escolar", typeof(IExecutarGravarHistoricoEscolarObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.ExclusaoCompensacaoAusenciaAlunoEAula, new ComandoRabbit("Executa exclusão lógica de compensações de ausências aluno e aula por aula", typeof(IExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgp.ExclusaoCompensacaoAusenciaPorIds, new ComandoRabbit("Executa exclusão lógica de compensações de ausências que não tem compensação ausência aluno e aula por ids", typeof(IExcluirCompensacaoAusenciaPorIdsUseCase)));
            Comandos.Add(RotasRabbitSgp.ExecutarAtualizacaoDasInformacoesPlanoAEE, new ComandoRabbit("Atualiza informações do plano AEE", typeof(IAtualizarInformacoesDoPlanoAEEUseCase), true));
            Comandos.Add(RotasRabbitSgp.ExecutarAtualizacaoDaTurmaDoPlanoAEE, new ComandoRabbit("Atualiza turma do planoAEE", typeof(IAtualizarTurmaDoPlanoAEEUseCase), true));
            Comandos.Add(RotasRabbitSgp.ExecutarMigracaoRelatorioSemestralPAP, new ComandoRabbit("Executar migração do relatório semestral pap", typeof(IExecutarMigracaoRelatorioSemestralPAPUseCase), true));
            Comandos.Add(RotasRabbitSgp.ExecutarMigracaoRelatorioSemestralPAPPorAnoLetivo, new ComandoRabbit("Executar migração do relatório semestral pap por ano letivo", typeof(IExecutarMigracaoRelatorioPapPorAnoEletivoUseCase), true));
            Comandos.Add(RotasRabbitSgp.ExecutarMigracaoRelatorioSemestralPAPPorId, new ComandoRabbit("Executar migração do relatório semestral pap por id", typeof(IExecutarMigracaoRelatorioPAPPorIdUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoInformativo, new ComandoRabbit("Executar notificação de informativo aos usuários", typeof(IExecutarNotificacaoInformativoUsuariosUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoInformativoUsuario, new ComandoRabbit("Executar notificação de informativo ao usuário", typeof(IExecutarNotificacaoInformativoUsuarioUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoInformativo, new ComandoRabbit("Executar exclusão notificações de informativo aos usuários", typeof(IExecutarExclusaoNotificacaoInformativoUsuariosUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoInformativoUsuario, new ComandoRabbit("Executar exclusão notificação de informativo ao usuário", typeof(IExecutarExclusaoNotificacaoInformativoUsuarioUseCase), true));
            Comandos.Add(RotasRabbitSgp.GerarCacheAtribuicaoResponsaveis, new ComandoRabbit("Gerar cache atribuições de responsáveis ativos", typeof(IGerarCacheAtribuicaoResponsaveisUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaExecutarExclusaoDasNotificacoesPeriodicamente, new ComandoRabbit("Executar exclusão notificações peridicamente", typeof(IExecutarExclusaoNotificacoesPeriodicamenteUseCase), true));
            Comandos.Add(RotasRabbitSgp.RotaExecutarExclusaoDaNotificacao, new ComandoRabbit("Executar exclusão notificação", typeof(IExecutarExclusaoNotificacaoUseCase), true));
            Comandos.Add(RotasRabbitSgp.ExecutarAtualizacaoMapeamentoEstudantes, new ComandoRabbit("Executar atualização do mapeamento dos estudantes", typeof(IAtualizarMapeamentoDosEstudantesUseCase), true));
            Comandos.Add(RotasRabbitSgp.ExecutarAtualizacaoMapeamentoEstudantesBimestre, new ComandoRabbit("Executar atualização do mapeamento do estudante do bimestre", typeof(IAtualizarMapeamentoDoEstudanteDoBimestreUseCase), true));
            Comandos.Add(RotasRabbitSgp.SincronizarAbrangencia, new ComandoRabbit("Inicia o processo de sincronização", typeof(IAbrangenciaSyncUseCase)));
        }
    }
}
