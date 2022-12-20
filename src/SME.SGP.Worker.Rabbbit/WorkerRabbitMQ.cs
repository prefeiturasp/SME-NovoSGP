using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System.Linq;

namespace SME.SGP.Worker.RabbitMQ
{
    public class WorkerRabbitMQ : WorkerRabbitMQBase
    {
        public WorkerRabbitMQ(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria,
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
            Comandos.Add(RotasRabbitSgp.RotaRelatoriosProntos, new ComandoRabbit("Receber dados do relatório", typeof(IReceberRelatorioProntoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRelatoriosProntosApp, new ComandoRabbit("Receber dados do relatório do Escola Aqui", typeof(IReceberRelatorioProntoEscolaAquiUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRelatorioCorrelacaoCopiar, new ComandoRabbit("Copiar e gerar novo código de correlação", typeof(ICopiarCodigoCorrelacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRelatorioCorrelacaoInserir, new ComandoRabbit("Inserir novo código de correlação", typeof(IInserirCodigoCorrelacaoUseCase)));

            RegistrarUseCasesRelatorioComErro();
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            RegistrarUseCasesRelatorios();

            Comandos.Add(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo, new ComandoRabbit("Notificar usuário sobre nova observação no diário de bordo", typeof(INotificarDiarioBordoObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoAlterarObservacaoDiarioBordo, new ComandoRabbit("Alterar as notificações dos usuários excluídos das observação no diário de bordo", typeof(IAlterarNotificacaoObservacaoDiarioBordoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNovaNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Notificar usuário sobre nova observação na carta de intenções", typeof(ISalvarNotificacaoCartaIntencoesObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Excluir uma notificação sobre observação na carta de intenções", typeof(IExcluirNotificacaoCartaIntencoesObservacaoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaNovaNotificacaoDevolutiva, new ComandoRabbit("Notificar usuário sobre a criação de uma devolutiva", typeof(ISalvarNotificacaoDevolutivaUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoDevolutiva, new ComandoRabbit("Excluir uma notificação sobre devolutiva", typeof(IExcluirNotificacaoDevolutivaUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoDiarioBordo, new ComandoRabbit("Excluir uma notificação do diario de bordo", typeof(IExcluirNotificacaoDiarioBordoUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaSincronizaComponetesCurricularesEol, new ComandoRabbit("Sincroniza os compoentes curriculares com o Eol", typeof(ISincronizarComponentesCurricularesUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes, new ComandoRabbit("Executa exclusão de pendências de dias letivos insuficientes", typeof(IExecutarExclusaoPendenciaDiasLetivosInsuficientes)));
            Comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciaParametroEvento, new ComandoRabbit("Executa exclusão de pendências de eventos por parâmetro", typeof(IExecutarExclusaoPendenciaParametroEvento)));
            Comandos.Add(RotasRabbitSgp.RotaNotificacaoReuniaoPedagogica, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoReuniaoPedagogicaUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaTrataNotificacoesNiveis, new ComandoRabbit("Trata Níveis e Cargos das notificações aguardando ação", typeof(ITrataNotificacoesNiveisCargosUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Gerar as pendências por ausência de registro individual", typeof(IGerarPendenciaAusenciaRegistroIndividualUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaAtualizarPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Atualizar pendência por ausência de registro individual", typeof(IAtualizarPendenciaRegistroIndividualUseCase)));
            Comandos.Add(RotasRabbitSgp.NotificarCompensacaoAusencia, new ComandoRabbit("Notificar Compensação Ausência", typeof(INotificarCompensacaoAusenciaUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantil, new ComandoRabbit("Consolidar Devolutivas Turmas da Modalidade Infantil", typeof(IConsolidarDevolutivasPorTurmaInfantilUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new ComandoRabbit("Consolidar Devolutivas Turmas ", typeof(IConsolidarDevolutivasPorTurmaUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarDiariosBordoCarregar, new ComandoRabbit("Carregar UEs para consolidação de Diarios de Bordo", typeof(IConsolidarDiariosBordoCarregarUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarDiariosBordoPorUeTratar, new ComandoRabbit("Tratar consolidação de diarios de bordo por UE", typeof(IConsolidarDiariosBordoPorUeTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorUeTratar, new ComandoRabbit("Tratar consolidação de registros pedagógicos por UE", typeof(IConsolidarRegistrosPedagogicosPorUeTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorTurmaTratar, new ComandoRabbit("Tratar consolidação de registros pedagógicos por turma", typeof(IConsolidarRegistrosPedagogicosPorTurmaTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRabbitDeadletterSync, new ComandoRabbit("Rotina para verificação das rotas de dead letter", typeof(IRabbitDeadletterSgpSyncUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRabbitDeadletterTratar, new ComandoRabbit("Rotina para verificação das rotas de dead letter", typeof(IRabbitDeadletterSgpTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaRabbitSRDeadletterTratar, new ComandoRabbit("Rotina para verificação das rotas de dead letter", typeof(IRabbitDeadletterSrTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarMediaRegistrosIndividuaisTurma, new ComandoRabbit("Consolidar Média de Registros Individuais", typeof(IConsolidacaoMediaRegistrosIndividuaisTurmaUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarMediaRegistrosIndividuais, new ComandoRabbit("Consolidar Média de Registros Individuais", typeof(IConsolidacaoMediaRegistrosIndividuaisUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAluno, new ComandoRabbit("Consolidar Acompanhamento Aprendizagem", typeof(IConsolidacaoAcompanhamentoAprendizagemAlunosSyncUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoPorUE, new ComandoRabbit("Consolidar Acompanhamento Aprendizagem por UE", typeof(IConsolidacaoAcompanhamentoAprendizagemAlunosPorUEUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAlunoTratar, new ComandoRabbit("Tratar Acompanhamento Aprendizagem", typeof(IConsolidacaoAcompanhamentoAprendizagemAlunosTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.ConsolidarRegistrosPedagogicos, new ComandoRabbit("Consolidar Registros Pedagógicos", typeof(IConsolidarRegistrosPedagogicosUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaAgendamentoTratar, new ComandoRabbit("Tratar mensagens de processamento periodico", typeof(IRotasAgendamentoTratarUseCase)));
            Comandos.Add(RotasRabbitSgp.WorkflowAprovacaoExcluir, new ComandoRabbit("Executar exclusão de workflow de aprovação por id", typeof(IExcluirWorkflowAprovacaoPorIdUseCase)));
            Comandos.Add(RotasRabbitSgp.DiarioBordoDaAulaExcluir, new ComandoRabbit("Executar exclusão de diario de bordo por aula id", typeof(IExcluirDiarioBordoPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgp.ExecutarTipoCalendario, new ComandoRabbit("Executar o tipo calendario", typeof(IExecutarTipoCalendarioUseCase)));
            Comandos.Add(RotasRabbitSgp.ExecutarGravarRecorrencia, new ComandoRabbit("Executar a gravação das recorrencias", typeof(IExecutarGravarRecorrenciaUseCase)));
            Comandos.Add(RotasRabbitSgp.AlterarRecorrenciaEventos, new ComandoRabbit("Altera recorrencia de eventos", typeof(IAlterarRecorrenciaEventosUseCase)));
            Comandos.Add(RotasRabbitSgp.SincronizarObjetivosComJurema, new ComandoRabbit("Realiza sincronização dos objetivos com Jurema", typeof(IExecutarSincronizacaoObjetivosComJuremaUseCase)));
            Comandos.Add(RotasRabbitSgp.NotificarAlunosFaltososBimestre, new ComandoRabbit("Notifica os alunos faltoso no bimestre", typeof(IExecutarNotificacaoAlunosFaltososBimestreUseCase)));
            Comandos.Add(RotasRabbitSgp.NotificacoesNiveisCargos, new ComandoRabbit("Sicronizar componentes curriculares", typeof(IExecutarTratamentoNotificacoesNiveisCargosUseCase)));
            Comandos.Add(RotasRabbitSgp.SincronizarComponentesCurriculares, new ComandoRabbit("Sicronizar componentes curriculares", typeof(IExecutarSincronismoComponentesCurricularesUseCase)));
            Comandos.Add(RotasRabbitSgp.SincronizarComponentesCurricularesEol, new ComandoRabbit("Sicronizar componentes curriculares eol", typeof(IExecutaSincronismoComponentesCurricularesEolUseCase)));
            Comandos.Add(RotasRabbitSgp.SyncGeralGoogleClassroom, new ComandoRabbit("Sicronizar geral google classroom", typeof(IExecutarSyncGeralGoogleClassroomUseCase)));
            Comandos.Add(RotasRabbitSgp.SyncGsaGoogleClassroom, new ComandoRabbit("Sicronizar gsa google classroom", typeof(IExecutaSyncGsaGoogleClassroomUseCase)));
            Comandos.Add(RotasRabbitSgp.SyncSerapEstudantesProvas, new ComandoRabbit("Sicronizar provas dos estantes serap ", typeof(IExecutarSyncSerapEstudantesProvasUseCase)));
            Comandos.Add(RotasRabbitSgp.TratarNotificacoesNiveisCargos, new ComandoRabbit("Tratar notificações", typeof(ITrataNotificacoesNiveisCargosUseCase)));
            Comandos.Add(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoCarregar, new ComandoRabbit("Efetua a atualização da rota das imagens do acompanhamento aluno", typeof(ICarregarAjusteImagensAcompanhamentoAprendizagemAlunoUseCase)));
            Comandos.Add(RotasRabbitSgp.AjusteImagesAcompanhamentoAprendizagemAlunoSync, new ComandoRabbit("Efetua a atualização da rota das imagens do acompanhamento aluno", typeof(ISyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase)));
            Comandos.Add(RotasRabbitSgp.AtualizaUltimoLoginUsuario, new ComandoRabbit("Efetua a atualização da data de ultimo login do usuario", typeof(IAtualizarUltimoLoginUsuarioUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorDre, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por DRE", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorUe, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por UE", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorTurma, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por Turma", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase)));
            Comandos.Add(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorComponente, new ComandoRabbit("Verificar se existe Pendências de Devolutivas por Componente", typeof(IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase)));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveis, new ComandoRabbit("Remover Atribuição de Responsaveis por DRE", typeof(IRemoverAtribuicaoResponsaveisUseCase)));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveisSupervisorPorDre, new ComandoRabbit("Remover Atribuição de Responsaveis - Supervisor por DRE", typeof(IRemoverAtribuicaoResponsaveisSupervisorPorDreUseCase)));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveisPAAIPorDre, new ComandoRabbit("Remover Atribuição de Responsaveis - PAAI por DRE", typeof(IRemoverAtribuicaoResponsaveisPAAIPorDreUseCase)));
            Comandos.Add(RotasRabbitSgp.RemoverAtribuicaoResponsaveisASPPorDre, new ComandoRabbit("Remover Atribuição de Responsaveis - Assistente Social, Psicólogo ou Psicopedagogo por DRE", typeof(IRemoverAtribuicaoResponsaveisASPPPorDreUseCase)));
            Comandos.Add(RotasRabbitSgp.RemoverArquivoArmazenamento, new ComandoRabbit("Excluir Arquivo Armazenado no Minio", typeof(IExcluirArmazenamentoPorAquivoUseCase)));
        }
    }
}
