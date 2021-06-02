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
        private readonly ConfiguracaoRabbitOptions consumoDeFilasOptions;

        /// <summary>
        /// configuração da lista de tipos para a fila do rabbit instanciar, seguindo a ordem de propriedades:
        /// rota do rabbit, usaMediatr?, tipo
        /// </summary>
        private readonly Dictionary<string, ComandoRabbit> comandos;


        public WorkerRabbitMQ(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ConfiguracaoRabbitOptions consumoDeFilasOptions)
        {
            sentryDSN = configuration.GetValue<string>("Sentry:DSN");
            
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

            this.consumoDeFilasOptions = consumoDeFilasOptions;            

            var factory = new ConnectionFactory
            {
                HostName = this.consumoDeFilasOptions.HostName,  
                UserName = this.consumoDeFilasOptions.UserName, 
                Password = this.consumoDeFilasOptions.Password, 
                VirtualHost = this.consumoDeFilasOptions.VirtualHost 
            };

            conexaoRabbit = factory.CreateConnection();
            canalRabbit = conexaoRabbit.CreateModel();
            
            canalRabbit.BasicQos(0, this.consumoDeFilasOptions.LimiteDeMensagensPorExecucao, false);

            canalRabbit.ExchangeDeclare(RotasRabbit.ExchangeSgp, ExchangeType.Topic);

            canalRabbit.ExchangeDeclare(RotasRabbit.ExchangeServidorRelatorios, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbit.FilaSgp, false, false, false, null);

            canalRabbit.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeServidorRelatorios, "*", null);
            canalRabbit.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, "*");

            canalRabbit.QueueDeclare(RotasRabbit.WorkerRelatoriosSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbit.WorkerRelatoriosSgp, RotasRabbit.ExchangeServidorRelatorios, "*", null);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalDreSync, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalDreSync, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalDreSync);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalDreTratar);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalUeTratar);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaTratar, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaTratar, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaTratar);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalCicloSync, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalCicloSync, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalCicloSync);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaEstruturaInstitucionalCicloTratar, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaEstruturaInstitucionalCicloTratar, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaEstruturaInstitucionalCicloTratar);

            canalRabbit.QueueDeclare(RotasRabbit.ConsolidarFrequenciasTurmasNoAno, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.ConsolidarFrequenciasTurmasNoAno, RotasRabbit.ExchangeSgp, RotasRabbit.ConsolidarFrequenciasTurmasNoAno);

            canalRabbit.QueueDeclare(RotasRabbit.ConsolidarFrequenciasPorTurma, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.ConsolidarFrequenciasPorTurma, RotasRabbit.ExchangeSgp, RotasRabbit.ConsolidarFrequenciasPorTurma);

            canalRabbit.QueueDeclare(RotasRabbit.SincronizaDevolutivasPorTurmaInfantilSync, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.SincronizaDevolutivasPorTurmaInfantilSync, RotasRabbit.ExchangeSgp, RotasRabbit.SincronizaDevolutivasPorTurmaInfantilSync);

            canalRabbit.QueueDeclare(RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil, true, false, false);
            canalRabbit.QueueBind(RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil, RotasRabbit.ExchangeSgp, RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil);

            comandos = new Dictionary<string, ComandoRabbit>();
            RegistrarUseCases();
        }

        private void RegistrarUseCases()
        {
            comandos.Add(RotasRabbit.RotaRelatoriosProntos, new ComandoRabbit("Receber dados do relatório", typeof(IReceberRelatorioProntoUseCase)));
            comandos.Add(RotasRabbit.RotaInserirAulaRecorrencia, new ComandoRabbit("Inserir aulas recorrentes", typeof(IInserirAulaRecorrenteUseCase)));
            comandos.Add(RotasRabbit.RotaAlterarAulaRecorrencia, new ComandoRabbit("Alterar aulas recorrentes", typeof(IAlterarAulaRecorrenteUseCase)));
            comandos.Add(RotasRabbit.RotaExcluirAulaRecorrencia, new ComandoRabbit("Excluir aulas recorrentes", typeof(IExcluirAulaRecorrenteUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoUsuario, new ComandoRabbit("Notificar usuário", typeof(INotificarUsuarioUseCase)));
            comandos.Add(RotasRabbit.RotaRelatorioComErro, new ComandoRabbit("Notificar relatório com erro", typeof(IReceberRelatorioComErroUseCase)));
            comandos.Add(RotasRabbit.RotaRelatorioCorrelacaoCopiar, new ComandoRabbit("Copiar e gerar novo código de correlação", typeof(ICopiarCodigoCorrelacaoUseCase)));
            comandos.Add(RotasRabbit.RotaRelatorioCorrelacaoInserir, new ComandoRabbit("Inserir novo código de correlação", typeof(IInserirCodigoCorrelacaoUseCase)));
            comandos.Add(RotasRabbit.RotaSincronizarAulasInfatil, new ComandoRabbit("Sincronizar aulas da modalidade Infantil que devem ser criadas ou excluídas", typeof(ICriarAulasInfantilAutomaticamenteUseCase)));
            comandos.Add(RotasRabbit.RotaCriarAulasInfatilAutomaticamente, new ComandoRabbit("Criar aulas da modalidade Infantil automaticamente", typeof(ICriarAulasInfantilUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoExclusaoAulasComFrequencia, new ComandoRabbit("Notificar usuário sobre a exclusão de aulas com frequência registrada", typeof(INotificarExclusaoAulaComFrequenciaUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoNovaObservacaoDiarioBordo, new ComandoRabbit("Notificar usuário sobre nova observação no diário de bordo", typeof(INotificarDiarioBordoObservacaoUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoAlterarObservacaoDiarioBordo, new ComandoRabbit("Alterar as notificações dos usuários excluídos das observação no diário de bordo", typeof(IAlterarNotificacaoObservacaoDiarioBordoUseCase)));
            comandos.Add(RotasRabbit.RotaNovaNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Notificar usuário sobre nova observação na carta de intenções", typeof(ISalvarNotificacaoCartaIntencoesObservacaoUseCase)));
            comandos.Add(RotasRabbit.RotaExcluirNotificacaoObservacaoCartaIntencoes, new ComandoRabbit("Excluir uma notificação sobre observação na carta de intenções", typeof(IExcluirNotificacaoCartaIntencoesObservacaoUseCase)));
            comandos.Add(RotasRabbit.RotaNovaNotificacaoDevolutiva, new ComandoRabbit("Notificar usuário sobre a criação de uma devolutiva", typeof(ISalvarNotificacaoDevolutivaUseCase)));
            comandos.Add(RotasRabbit.RotaExcluirNotificacaoDevolutiva, new ComandoRabbit("Excluir uma notificação sobre devolutiva", typeof(IExcluirNotificacaoDevolutivaUseCase)));
            comandos.Add(RotasRabbit.RotaExcluirNotificacaoDiarioBordo, new ComandoRabbit("Excluir uma notificação do diario de bordo", typeof(IExcluirNotificacaoDiarioBordoUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaPendenciasAula, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaUseCase)));
            comandos.Add(RotasRabbit.RotaSincronizaComponetesCurricularesEol, new ComandoRabbit("Sincroniza os compoentes curriculares com o Eol", typeof(ISincronizarComponentesCurricularesUseCase)));
            comandos.Add(RotasRabbit.RotaCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase)));

            comandos.Add(RotasRabbit.RotaExecutaVerificacaoPendenciasGerais, new ComandoRabbit("Executa verficação das pendências gerais", typeof(IExecutaVerificacaoPendenciasGeraisUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaExclusaoPendenciasAula, new ComandoRabbit("Executa exclusão de pendências da aula", typeof(IExecutarExclusaoPendenciasAulaUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes, new ComandoRabbit("Executa exclusão de pendências de dias letivos insuficientes", typeof(IExecutarExclusaoPendenciaDiasLetivosInsuficientes)));
            comandos.Add(RotasRabbit.RotaExecutaExclusaoPendenciaParametroEvento, new ComandoRabbit("Executa exclusão de pendências de eventos por parâmetro", typeof(IExecutarExclusaoPendenciaParametroEvento)));
            comandos.Add(RotasRabbit.RotaExecutaExclusaoPendenciasAusenciaAvaliacao, new ComandoRabbit("Executa exclusão de pendências de ausencia de avaliação", typeof(IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaVerificacaoPendenciasProfessor, new ComandoRabbit("Executa verificação de pendências de avaliação do professor", typeof(IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaVerificacaoPendenciasAusenciaFechamento, new ComandoRabbit("Executa verificação de pendências de fechamento de bimestre", typeof(IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaExclusaoPendenciasAusenciaFechamento, new ComandoRabbit("Executa exclusão de pendências de ausencia de fechamento", typeof(IExecutarExclusaoPendenciasAusenciaFechamentoUseCase)));

            comandos.Add(RotasRabbit.RotaNotificacaoResultadoInsatisfatorio, new ComandoRabbit("Notificar usuário resultado insatisfatório de aluno", typeof(INotificarResultadoInsatisfatorioUseCase)));
            comandos.Add(RotasRabbit.RotaExecutaAtualizacaoSituacaoConselhoClasse, new ComandoRabbit("Executa atualização da situação do conselho de classe", typeof(IAtualizarSituacaoConselhoClasseUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoAndamentoFechamento, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoAndamentoFechamentoUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoUeFechamentosInsuficientes, new ComandoRabbit("Executa notificação UE sobre fechamento insuficientes", typeof(INotificacaoUeFechamentosInsuficientesUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoReuniaoPedagogica, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoReuniaoPedagogicaUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoPeriodoFechamento, new ComandoRabbit("Executa notificação sobre período de fechamento", typeof(INotificacaoPeriodoFechamentoUseCase)));

            comandos.Add(RotasRabbit.RotaNotificacaoInicioFimPeriodoFechamento, new ComandoRabbit("Executa notificação sobre o início e fim do Periodo de fechamento", typeof(INotificacaoInicioFimPeriodoFechamentoUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoFrequenciaUe, new ComandoRabbit("Notificar frequências dos alunos no bimestre para UE", typeof(INotificacaoFrequenciaUeUseCase)));

            comandos.Add(RotasRabbit.RotaTrataNotificacoesNiveis, new ComandoRabbit("Trata Níveis e Cargos das notificações aguardando ação", typeof(ITrataNotificacoesNiveisCargosUseCase)));
            comandos.Add(RotasRabbit.RotaPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Gerar as pendências por ausência de registro individual", typeof(IGerarPendenciaAusenciaRegistroIndividualUseCase)));
            comandos.Add(RotasRabbit.RotaAtualizarPendenciaAusenciaRegistroIndividual, new ComandoRabbit("Atualizar pendência por ausência de registro individual", typeof(IAtualizarPendenciaRegistroIndividualUseCase)));

            comandos.Add(RotasRabbit.RotaNotificacaoRegistroConclusaoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para registro de conclusão do Encaminhamento AEE", typeof(INotificacaoConclusaoEncaminhamentoAEEUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoEncerramentoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para encerramento do Encaminhamento AEE", typeof(INotificacaoEncerramentoEncaminhamentoAEEUseCase)));
            comandos.Add(RotasRabbit.RotaNotificacaoDevolucaoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para devolucao do Encaminhamento AEE", typeof(INotificacaoDevolucaoEncaminhamentoAEEUseCase)));

            comandos.Add(RotasRabbit.EncerrarPlanoAEEEstudantesInativos, new ComandoRabbit("Excluir plano AEE estudantes inativos", typeof(IEncerrarPlanosAEEEstudantesInativosUseCase)));
            comandos.Add(RotasRabbit.GerarPendenciaValidadePlanoAEE, new ComandoRabbit("Gerar Pendência de Validade do PlanoAEE", typeof(IGerarPendenciaValidadePlanoAEEUseCase)));
            comandos.Add(RotasRabbit.NotificarPlanoAEEExpirado, new ComandoRabbit("Excluir plano AEE estudantes inativos", typeof(INotificarPlanosAEEExpiradosUseCase)));
            comandos.Add(RotasRabbit.NotificarPlanoAEEEmAberto, new ComandoRabbit("Notificar Plano AEE que estejam abertos", typeof(INotificarPlanosAEEEmAbertoUseCase)));

            comandos.Add(RotasRabbit.NotificarPlanoAEEReestruturado, new ComandoRabbit("Enviar Notificação de Reestruturação de PlanoAEE", typeof(IEnviarNotificacaoReestruturacaoPlanoAEEUseCase)));
            comandos.Add(RotasRabbit.NotificarCriacaoPlanoAEE, new ComandoRabbit("Enviar Notificação de Criação de PlanoAEE", typeof(IEnviarNotificacaoCriacaoPlanoAEEUseCase)));
            comandos.Add(RotasRabbit.NotificarPlanoAEEEncerrado, new ComandoRabbit("Enviar Notificação de Encerramento de PlanoAEE", typeof(IEnviarNotificacaoEncerramentoPlanoAEEUseCase)));

            // Sincronização das UES e turmas
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalDreSync, new ComandoRabbit("Estrutura Institucional - Sync de Dre", typeof(IExecutarSincronizacaoInstitucionalDreSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Dre", typeof(IExecutarSincronizacaoInstitucionalDreTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Ue", typeof(IExecutarSincronizacaoInstitucionalUeTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync, new ComandoRabbit("Estrutura Institucional - Sync de Tipos de Escola", typeof(IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaTratar, new ComandoRabbit("Estrutura Institucional - Tratar um Tipo de Escola", typeof(IExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalCicloSync, new ComandoRabbit("Estrutura Institucional - Sync de Ciclo Ensino", typeof(IExecutarSincronizacaoInstitucionalCicloSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalCicloTratar, new ComandoRabbit("Estrutura Institucional - Tratar um de Ciclo Ensino", typeof(IExecutarSincronizacaoInstitucionalCicloTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Turmas", typeof(IExecutarSincronizacaoInstitucionalTurmaSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Turma", typeof(IExecutarSincronizacaoInstitucionalTurmaTratarUseCase)));
           
            comandos.Add(RotasRabbit.RotaNotificacaoRegistroItineranciaInseridoUseCase, new ComandoRabbit("Enviar Notificação quanto insere um novo Registro de Itinerância", typeof(INotificacaoSalvarItineranciaUseCase)));

            comandos.Add(RotasRabbit.ConsolidacaoFrequenciasTurmasCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas - Carregar", typeof(IConsolidarFrequenciaTurmasUseCase)));
            comandos.Add(RotasRabbit.ConsolidarFrequenciasTurmasNoAno, new ComandoRabbit("Consolidar Registros de Frequência das Turmas", typeof(IConsolidarFrequenciaTurmasPorAnoUseCase)));
            comandos.Add(RotasRabbit.ConsolidarFrequenciasPorTurma, new ComandoRabbit("Consolidar Registros de Frequência por Turma", typeof(IConsolidarFrequenciaPorTurmaUseCase)));

            // Sincronização de Devolutivas
            comandos.Add(RotasRabbit.SincronizaDevolutivasPorTurmaInfantilSync, new ComandoRabbit("Devolutivas - Sync de Turmas da Modalidade Infantil", typeof(IExecutarSincronizacaoDevolutivasPorTurmaInfantilSyncUseCase)));
            comandos.Add(RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil, new ComandoRabbit("Consolidar Devolutivas Turmas da Modalidade Infantil", typeof(IConsolidarDevolutivasPorTurmaInfantilUseCase)));

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
                        canalRabbit.BasicReject(ea.DeliveryTag, false);
                        SentrySdk.AddBreadcrumb($"Erros: {nex.Message}" ,null, null, null, BreadcrumbLevel.Error);
                        SentrySdk.CaptureException(nex);
                        RegistrarSentry(ea, mensagemRabbit, nex);
                        if (mensagemRabbit.NotificarErroUsuario)
                            NotificarErroUsuario(nex.Message, mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                    }
                    catch (ValidacaoException vex)
                    {
                        canalRabbit.BasicReject(ea.DeliveryTag, false);
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

                canalRabbit.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, RotasRabbit.RotaNotificacaoUsuario);
                canalRabbit.BasicPublish(RotasRabbit.ExchangeSgp, RotasRabbit.RotaNotificacaoUsuario, null, body);
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

            canalRabbit.BasicConsume(RotasRabbit.FilaSgp, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalDreSync, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaSync, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalTipoEscolaTratar, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalCicloSync, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalCicloTratar, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.ConsolidarFrequenciasTurmasNoAno, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.ConsolidarFrequenciasPorTurma, false, consumer);
            // Consolidação de Devolutivas
            canalRabbit.BasicConsume(RotasRabbit.SincronizaDevolutivasPorTurmaInfantilSync, false, consumer);
            canalRabbit.BasicConsume(RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil, false, consumer);

            return Task.CompletedTask;
        }
    }
}
