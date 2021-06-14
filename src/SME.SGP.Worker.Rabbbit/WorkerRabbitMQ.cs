using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sentry;
using Sentry.Protocol;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Excecoes;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Worker.RabbitMQ
{
    public class WorkerRabbitMQ : IHostedService
    {
        private readonly IModel canalRabbit;
        private readonly string sentryDSN;
        private readonly IConnection conexaoRabbit;
        private readonly IServiceScopeFactory serviceScopeFactory;        

        /// <summary>
        /// configuração da lista de tipos para a fila do rabbit instanciar, seguindo a ordem de propriedades:
        /// rota do rabbit, usaMediatr?, tipo
        /// </summary>
        private readonly Dictionary<string, ComandoRabbit> comandos;

        public WorkerRabbitMQ(IConnection conexaoRabbit, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            sentryDSN = configuration.GetValue<string>("Sentry:DSN");
            this.conexaoRabbit = conexaoRabbit ?? throw new ArgumentNullException(nameof(conexaoRabbit));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, 10, false);

            canalRabbit.ExchangeDeclare(ExchangeRabbit.Sgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeRabbit.SgpDeadLetter, ExchangeType.Direct, true, false);

            DeclararFilasSgp();

            comandos = new Dictionary<string, ComandoRabbit>();
            RegistrarUseCases();
        }

        private void DeclararFilasSgp()
        {
            foreach (var fila in typeof(RotasRabbitSgp).ObterConstantesPublicas<string>())
            {
                var args = new Dictionary<string, object>()
                    {
                        { "x-dead-letter-exchange", ExchangeRabbit.SgpDeadLetter }
                    };

                canalRabbit.QueueDeclare(fila, true, false, false, args);
                canalRabbit.QueueBind(fila, ExchangeRabbit.Sgp, fila, null);

                var filaDeadLetter = $"{fila}.deadletter";
                canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, null);
                canalRabbit.QueueBind(filaDeadLetter, ExchangeRabbit.SgpDeadLetter, fila, null);
            }
        }

        private void RegistrarUseCases()
        {
            comandos.Add(RotasRabbitSgp.RotaRelatoriosProntos, new ComandoRabbit("Receber dados do relatório", typeof(IReceberRelatorioProntoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaInserirAulaRecorrencia, new ComandoRabbit("Inserir aulas recorrentes", typeof(IInserirAulaRecorrenteUseCase)));
            comandos.Add(RotasRabbitSgp.RotaAlterarAulaRecorrencia, new ComandoRabbit("Alterar aulas recorrentes", typeof(IAlterarAulaRecorrenteUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExcluirAulaRecorrencia, new ComandoRabbit("Excluir aulas recorrentes", typeof(IExcluirAulaRecorrenteUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoUsuario, new ComandoRabbit("Notificar usuário", typeof(INotificarUsuarioUseCase)));
            comandos.Add(RotasRabbitSgp.RotaRelatorioComErro, new ComandoRabbit("Notificar relatório com erro", typeof(IReceberRelatorioComErroUseCase)));
            comandos.Add(RotasRabbitSgp.RotaRelatorioCorrelacaoCopiar, new ComandoRabbit("Copiar e gerar novo código de correlação", typeof(ICopiarCodigoCorrelacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaRelatorioCorrelacaoInserir, new ComandoRabbit("Inserir novo código de correlação", typeof(IInserirCodigoCorrelacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaSincronizarAulasInfatil, new ComandoRabbit("Sincronizar aulas da modalidade Infantil que devem ser criadas ou excluídas", typeof(ICriarAulasInfantilAutomaticamenteUseCase)));
            comandos.Add(RotasRabbitSgp.RotaCriarAulasInfatilAutomaticamente, new ComandoRabbit("Criar aulas da modalidade Infantil automaticamente", typeof(ICriarAulasInfantilUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoExclusaoAulasComFrequencia, new ComandoRabbit("Notificar usuário sobre a exclusão de aulas com frequência registrada", typeof(INotificarExclusaoAulaComFrequenciaUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoNovaObservacaoDiarioBordo, new ComandoRabbit("Notificar usuário sobre nova observação no diário de bordo", typeof(INotificarDiarioBordoObservacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoAlterarObservacaoDiarioBordo, new ComandoRabbit("Alterar as notificações dos usuários excluídos das observação no diário de bordo", typeof(IAlterarNotificacaoObservacaoDiarioBordoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNovaNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Notificar usuário sobre nova observação na carta de intenções", typeof(ISalvarNotificacaoCartaIntencoesObservacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Excluir uma notificação sobre observação na carta de intenções", typeof(IExcluirNotificacaoCartaIntencoesObservacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNovaNotificacaoDevolutiva, new ComandoRabbit("Notificar usuário sobre a criação de uma devolutiva", typeof(ISalvarNotificacaoDevolutivaUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoDevolutiva, new ComandoRabbit("Excluir uma notificação sobre devolutiva", typeof(IExcluirNotificacaoDevolutivaUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExcluirNotificacaoDiarioBordo, new ComandoRabbit("Excluir uma notificação do diario de bordo", typeof(IExcluirNotificacaoDiarioBordoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaPendenciasAula, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaUseCase)));
            comandos.Add(RotasRabbitSgp.RotaSincronizaComponetesCurricularesEol, new ComandoRabbit("Sincroniza os compoentes curriculares com o Eol", typeof(ISincronizarComponentesCurricularesUseCase)));
            comandos.Add(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase)));

            comandos.Add(RotasRabbitSgp.RotaExecutaVerificacaoPendenciasGerais, new ComandoRabbit("Executa verficação das pendências gerais", typeof(IExecutaVerificacaoPendenciasGeraisUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAula, new ComandoRabbit("Executa exclusão de pendências da aula", typeof(IExecutarExclusaoPendenciasAulaUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes, new ComandoRabbit("Executa exclusão de pendências de dias letivos insuficientes", typeof(IExecutarExclusaoPendenciaDiasLetivosInsuficientes)));
            comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciaParametroEvento, new ComandoRabbit("Executa exclusão de pendências de eventos por parâmetro", typeof(IExecutarExclusaoPendenciaParametroEvento)));
            comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAusenciaAvaliacao, new ComandoRabbit("Executa exclusão de pendências de ausencia de avaliação", typeof(IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaVerificacaoPendenciasProfessor, new ComandoRabbit("Executa verificação de pendências de avaliação do professor", typeof(IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaVerificacaoPendenciasAusenciaFechamento, new ComandoRabbit("Executa verificação de pendências de fechamento de bimestre", typeof(IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaExclusaoPendenciasAusenciaFechamento, new ComandoRabbit("Executa exclusão de pendências de ausencia de fechamento", typeof(IExecutarExclusaoPendenciasAusenciaFechamentoUseCase)));

            comandos.Add(RotasRabbitSgp.RotaNotificacaoResultadoInsatisfatorio, new ComandoRabbit("Notificar usuário resultado insatisfatório de aluno", typeof(INotificarResultadoInsatisfatorioUseCase)));
            comandos.Add(RotasRabbitSgp.RotaExecutaAtualizacaoSituacaoConselhoClasse, new ComandoRabbit("Executa atualização da situação do conselho de classe", typeof(IAtualizarSituacaoConselhoClasseUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoAndamentoFechamento, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoAndamentoFechamentoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoUeFechamentosInsuficientes, new ComandoRabbit("Executa notificação UE sobre fechamento insuficientes", typeof(INotificacaoUeFechamentosInsuficientesUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoReuniaoPedagogica, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoReuniaoPedagogicaUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoPeriodoFechamento, new ComandoRabbit("Executa notificação sobre período de fechamento", typeof(INotificacaoPeriodoFechamentoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaGeracaoPendenciasFechamento, new ComandoRabbit("Executa inclusão de fila de Geração de Pendências do Fechamento", typeof(IGerarPendenciasFechamentoUseCase)));

            comandos.Add(RotasRabbitSgp.RotaNotificacaoInicioFimPeriodoFechamento, new ComandoRabbit("Executa notificação sobre o início e fim do Periodo de fechamento", typeof(INotificacaoInicioFimPeriodoFechamentoUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoFrequenciaUe, new ComandoRabbit("Notificar frequências dos alunos no bimestre para UE", typeof(INotificacaoFrequenciaUeUseCase)));

            comandos.Add(RotasRabbitSgp.RotaTrataNotificacoesNiveis, new ComandoRabbit("Trata Níveis e Cargos das notificações aguardando ação", typeof(ITrataNotificacoesNiveisCargosUseCase)));
            comandos.Add(RotasRabbitSgp.RotaPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Gerar as pendências por ausência de registro individual", typeof(IGerarPendenciaAusenciaRegistroIndividualUseCase)));
            comandos.Add(RotasRabbitSgp.RotaAtualizarPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Atualizar pendência por ausência de registro individual", typeof(IAtualizarPendenciaRegistroIndividualUseCase)));
                       
            
            comandos.Add(RotasRabbitSgp.RotaNotificacaoRegistroConclusaoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para registro de conclusão do Encaminhamento AEE", typeof(INotificacaoConclusaoEncaminhamentoAEEUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoEncerramentoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para encerramento do Encaminhamento AEE", typeof(INotificacaoEncerramentoEncaminhamentoAEEUseCase)));
            comandos.Add(RotasRabbitSgp.RotaNotificacaoDevolucaoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para devolucao do Encaminhamento AEE", typeof(INotificacaoDevolucaoEncaminhamentoAEEUseCase)));

            comandos.Add(RotasRabbitSgp.EncerrarPlanoAEEEstudantesInativos, new ComandoRabbit("Excluir plano AEE estudantes inativos", typeof(IEncerrarPlanosAEEEstudantesInativosUseCase)));
            comandos.Add(RotasRabbitSgp.GerarPendenciaValidadePlanoAEE, new ComandoRabbit("Gerar Pendência de Validade do PlanoAEE", typeof(IGerarPendenciaValidadePlanoAEEUseCase)));
            comandos.Add(RotasRabbitSgp.NotificarPlanoAEEExpirado, new ComandoRabbit("Excluir plano AEE estudantes inativos", typeof(INotificarPlanosAEEExpiradosUseCase)));
            comandos.Add(RotasRabbitSgp.NotificarPlanoAEEEmAberto, new ComandoRabbit("Notificar Plano AEE que estejam abertos", typeof(INotificarPlanosAEEEmAbertoUseCase)));

            comandos.Add(RotasRabbitSgp.NotificarPlanoAEEReestruturado, new ComandoRabbit("Enviar Notificação de Reestruturação de PlanoAEE", typeof(IEnviarNotificacaoReestruturacaoPlanoAEEUseCase)));
            comandos.Add(RotasRabbitSgp.NotificarCriacaoPlanoAEE, new ComandoRabbit("Enviar Notificação de Criação de PlanoAEE", typeof(IEnviarNotificacaoCriacaoPlanoAEEUseCase)));
            comandos.Add(RotasRabbitSgp.NotificarPlanoAEEEncerrado, new ComandoRabbit("Enviar Notificação de Encerramento de PlanoAEE", typeof(IEnviarNotificacaoEncerramentoPlanoAEEUseCase)));

            // Sincronização das UES e turmas
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalDreSync, new ComandoRabbit("Estrutura Institucional - Sync de Dre", typeof(IExecutarSincronizacaoInstitucionalDreSyncUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalDreTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Dre", typeof(IExecutarSincronizacaoInstitucionalDreTratarUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalUeTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Ue", typeof(IExecutarSincronizacaoInstitucionalUeTratarUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalTipoEscolaSync, new ComandoRabbit("Estrutura Institucional - Sync de Tipos de Escola", typeof(IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalTipoEscolaTratar, new ComandoRabbit("Estrutura Institucional - Tratar um Tipo de Escola", typeof(IExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalCicloSync, new ComandoRabbit("Estrutura Institucional - Sync de Ciclo Ensino", typeof(IExecutarSincronizacaoInstitucionalCicloSyncUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalCicloTratar, new ComandoRabbit("Estrutura Institucional - Tratar um de Ciclo Ensino", typeof(IExecutarSincronizacaoInstitucionalCicloTratarUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalTurmasSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Turmas", typeof(IExecutarSincronizacaoInstitucionalTurmaSyncUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizaEstruturaInstitucionalTurmaTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Turma", typeof(IExecutarSincronizacaoInstitucionalTurmaTratarUseCase)));
           
            comandos.Add(RotasRabbitSgp.RotaNotificacaoRegistroItineranciaInseridoUseCase, new ComandoRabbit("Enviar Notificação quanto insere um novo Registro de Itinerância", typeof(INotificacaoSalvarItineranciaUseCase)));

            comandos.Add(RotasRabbitSgp.ConsolidacaoFrequenciasTurmasCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas - Carregar", typeof(IConsolidarFrequenciaTurmasUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidarFrequenciasTurmasNoAno, new ComandoRabbit("Consolidar Registros de Frequência das Turmas", typeof(IConsolidarFrequenciaTurmasPorAnoUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidarFrequenciasPorTurma, new ComandoRabbit("Consolidar Registros de Frequência por Turma", typeof(IConsolidarFrequenciaPorTurmaUseCase)));

            comandos.Add(RotasRabbitSgp.ConsolidacaoMatriculasTurmasDreCarregar, new ComandoRabbit("Carrega os dados de todas as Dres para consolidação de matrículas", typeof(ICarregarDresConsolidacaoMatriculaUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizarDresMatriculasTurmas, new ComandoRabbit("Consolidação de matrículas por turmas - Sincronizar Dres", typeof(IExecutarSincronizacaoDresConsolidacaoMatriculasUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidacaoMatriculasTurmasCarregar, new ComandoRabbit("Consolidação de matrículas por turmas - Carregar", typeof(ICarregarMatriculaTurmaUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidacaoMatriculasTurmasSync, new ComandoRabbit("Consolidação de matrículas por turmas - Sincronizar", typeof(IExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase)));
            comandos.Add(RotasRabbitSgp.RotaAlterarAulaFrequenciaTratar, new ComandoRabbit("Normaliza as frequências quando há uma alteração de aula única", typeof(IAlterarAulaFrequenciaTratarUseCase)));
            
            // Consolidação fechamento turmas
            comandos.Add(RotasRabbitSgp.ConsolidarGeralSync, new ComandoRabbit("Consolidação turmas geral", typeof(IExecutarConsolidacaoTurmaGeralUseCase)));

            comandos.Add(RotasRabbitSgp.ConsolidarTurmaSync, new ComandoRabbit("Consolidação Fechamento/conselho - Consolidar Turmas", typeof(IExecutarConsolidacaoTurmaUseCase)));

            comandos.Add(RotasRabbitSgp.ConsolidarTurmaConselhoClasseSync, new ComandoRabbit("Consolidação conselho classe - Sincronizar alunos da turma", typeof(IExecutarConsolidacaoTurmaConselhoClasseUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidarTurmaConselhoClasseAlunoTratar, new ComandoRabbit("Consolidação conselho classe - Consolidar aluno da turma", typeof(IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase)));

            comandos.Add(RotasRabbitSgp.ConsolidarTurmaFechamentoSync, new ComandoRabbit("Consolidação fechamento - Sincronizar Componentes da Turma", typeof(IExecutarConsolidacaoTurmaFechamentoUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidarTurmaFechamentoComponenteTratar, new ComandoRabbit("Consolidação fechamento - Consolidar Componentes da Turma", typeof(IExecutarConsolidacaoTurmaFechamentoComponenteUseCase)));            
            comandos.Add(RotasRabbitSgp.RotaValidacaoAusenciaConciliacaoFrequenciaTurma, new ComandoRabbit("Validação de ausência para conciliação de frequência da turma", typeof(IValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase)));

            ///Consolidação de Devolutivas
            comandos.Add(RotasRabbitSgp.SincronizaDevolutivasPorTurmaInfantilSync, new ComandoRabbit("Devolutivas - Sync de Turmas da Modalidade Infantil", typeof(IExecutarSincronizacaoDevolutivasPorTurmaInfantilSyncUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantil, new ComandoRabbit("Consolidar Devolutivas Turmas da Modalidade Infantil", typeof(IConsolidarDevolutivasPorTurmaInfantilUseCase)));
            comandos.Add(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new ComandoRabbit("Consolidar Devolutivas Turmas ", typeof(IConsolidarDevolutivasPorTurmaUseCase)));

            comandos.Add(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasSync, new ComandoRabbit("Inicia rotina de cálculo de frequência da turma", typeof(IConciliacaoFrequenciaTurmasSyncUseCase)));

            comandos.Add(RotasRabbitSgp.RotaRabbitDeadletterSync, new ComandoRabbit("Validação de ausência para conciliação de frequência da turma", typeof(IRabbitDeadletterSyncUseCase)));
            comandos.Add(RotasRabbitSgp.RotaRabbitDeadletterTratar, new ComandoRabbit("Validação de ausência para conciliação de frequência da turma", typeof(IRabbitDeadletterTratarUseCase)));
            
            comandos.Add(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasAlunosSync, new ComandoRabbit("Conciliação de frequência da turma sync", typeof(IConciliacaoFrequenciaTurmasAlunosSyncUseCase)));
            comandos.Add(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasAlunosBuscar, new ComandoRabbit("Conciliação de frequência da turma buscar", typeof(IConciliacaoFrequenciaTurmasAlunosBuscarUseCase)));

            comandos.Add(RotasRabbitSgp.RotaNotificacaoAlunosFaltosos, new ComandoRabbit("Conciliação de frequência da turma buscar", typeof(INotificarAlunosFaltososUseCase)));

            comandos.Add(RotasRabbitSgp.SincronizarDadosAulasFrequenciaMigracao, new ComandoRabbit("Sincronizar - migração dados frequencia aulas", typeof(IExecutarSincronizacaoDadosAulasFrequenciaUseCase)));
            comandos.Add(RotasRabbitSgp.CarregarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Carregar- migração dados frequencia alunos", typeof(ICarregarRegistroFrequenciaAlunosUseCase)));
            comandos.Add(RotasRabbitSgp.SincronizarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Executar sincronização - migração dados frequencia alunos", typeof(IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase)));
        }
        

        private async Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;
            if (comandos.ContainsKey(rota))
            {
                using (SentrySdk.Init(sentryDSN))
                {
                    var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                    //SentrySdk.AddBreadcrumb($"Dados: {mensagemRabbit.Mensagem}");
                    var comandoRabbit = comandos[rota];
                    try
                    {
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            AtribuirContextoAplicacao(mensagemRabbit, scope);

                            //SentrySdk.CaptureMessage($"{mensagemRabbit.UsuarioLogadoRF} - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)} - EXECUTANDO - {ea.RoutingKey} - {DateTime.Now:dd/MM/yyyy HH:mm:ss}", SentryLevel.Debug);
                            var casoDeUso = scope.ServiceProvider.GetService(comandoRabbit.TipoCasoUso);

                            await ObterMetodo(comandoRabbit.TipoCasoUso, "Executar").InvokeAsync(casoDeUso, new object[] { mensagemRabbit });

                            //SentrySdk.CaptureMessage($"{mensagemRabbit.UsuarioLogadoRF} - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)} - SUCESSO - {ea.RoutingKey}", SentryLevel.Info);
                            canalRabbit.BasicAck(ea.DeliveryTag, false);
                        }
                    }
                    catch (NegocioException nex)
                    {
                        canalRabbit.BasicAck(ea.DeliveryTag, false);
                        SentrySdk.AddBreadcrumb($"Erros: {nex.Message}" ,null, null, null, BreadcrumbLevel.Error);
                        SentrySdk.CaptureException(nex);
                        RegistrarSentry(ea, mensagemRabbit, nex);
                        if (mensagemRabbit.NotificarErroUsuario)
                            NotificarErroUsuario(nex.Message, mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                    }
                    catch (ValidacaoException vex)
                    {
                        canalRabbit.BasicAck(ea.DeliveryTag, false);
                        SentrySdk.AddBreadcrumb($"Erros: {JsonConvert.SerializeObject(vex.Mensagens())}", null, null, null, BreadcrumbLevel.Error);
                        SentrySdk.CaptureException(vex);
                        RegistrarSentry(ea, mensagemRabbit, vex);
                        if (mensagemRabbit.NotificarErroUsuario)
                            NotificarErroUsuario($"Ocorreu um erro interno, por favor tente novamente", mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                    }
                    catch (Exception ex)
                    {
                        canalRabbit.BasicReject(ea.DeliveryTag, false);
                        SentrySdk.AddBreadcrumb($"Erros: {ex.Message}", null, null, null, BreadcrumbLevel.Error);
                        SentrySdk.CaptureException(ex);
                        RegistrarSentry(ea, mensagemRabbit, ex);
                        if (mensagemRabbit.NotificarErroUsuario)
                            NotificarErroUsuario($"Ocorreu um erro interno, por favor tente novamente", mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                    }
                }
            }
            else
                canalRabbit.BasicReject(ea.DeliveryTag, false);
        }

        private void RegistrarSentry(BasicDeliverEventArgs ea, MensagemRabbit mensagemRabbit, Exception ex)
        {
            SentrySdk.CaptureMessage($"{mensagemRabbit.UsuarioLogadoRF} - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)} - ERRO - {ea.RoutingKey}", SentryLevel.Error);
            SentrySdk.CaptureException(ex);
        }

        private static void AtribuirContextoAplicacao(MensagemRabbit mensagemRabbit, IServiceScope scope)
        {
            if (!string.IsNullOrWhiteSpace(mensagemRabbit.UsuarioLogadoRF))
            {
                var contextoAplicacao = scope.ServiceProvider.GetService<IContextoAplicacao>();
                var variaveis = new Dictionary<string, object>();
                variaveis.Add("NomeUsuario", mensagemRabbit.UsuarioLogadoNomeCompleto);
                variaveis.Add("UsuarioLogado", mensagemRabbit.UsuarioLogadoRF);
                variaveis.Add("RF", mensagemRabbit.UsuarioLogadoRF);
                variaveis.Add("login", mensagemRabbit.UsuarioLogadoRF);
                variaveis.Add("Claims", new List<InternalClaim> { new InternalClaim { Value = mensagemRabbit.PerfilUsuario, Type = "perfil" } });
                contextoAplicacao.AdicionarVariaveis(variaveis);
            }
        }

        private void NotificarErroUsuario(string message, string usuarioRf, string nomeProcesso)
        {
            if (!string.IsNullOrEmpty(usuarioRf))
            {
                var command = new NotificarUsuarioCommand($"Ocorreu um erro ao: '{nomeProcesso}'",
                                                          message,
                                                          usuarioRf,
                                                          NotificacaoCategoria.Aviso,
                                                          NotificacaoTipo.Worker);

                var request = new MensagemRabbit(string.Empty, command, Guid.NewGuid(), usuarioRf);
                var mensagem = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(mensagem);

                canalRabbit.BasicPublish(ExchangeRabbit.Sgp, RotasRabbitSgp.RotaNotificacaoUsuario, null, body);
            }
        }

        private MethodInfo ObterMetodo(Type objType, string method)
        {
            var executar = objType.GetMethod(method);

            if (executar == null)
            {
                foreach (var itf in objType.GetInterfaces())
                {
                    executar = ObterMetodo(itf, method);
                    if (executar != null)
                        break;
                }
            }

            return executar;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            canalRabbit.Close();
            conexaoRabbit.Close();
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(canalRabbit);
            
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    await TratarMensagem(ea);
                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb($"Erro ao tratar mensagem {ea.DeliveryTag}", "erro", null, null, BreadcrumbLevel.Error);
                    SentrySdk.CaptureException(ex);
                    canalRabbit.BasicReject(ea.DeliveryTag, false);
                }
            };

            RegistrarConsumerSgp(consumer);
            return Task.CompletedTask;
        }

        private void RegistrarConsumerSgp(EventingBasicConsumer consumer)
        {
            foreach (var fila in typeof(RotasRabbitSgp).ObterConstantesPublicas<string>())
                canalRabbit.BasicConsume(fila, false, consumer);
        }
    }
}
