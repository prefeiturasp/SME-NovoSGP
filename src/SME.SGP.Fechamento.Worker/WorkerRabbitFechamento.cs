using Elastic.Apm;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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

namespace SME.SGP.Fechamento.Worker
{
    public class WorkerRabbitFechamento : IHostedService
    {
        private readonly IModel canalRabbit;
        private readonly IConnection conexaoRabbit;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly TelemetriaOptions telemetriaOptions;
        private IMediator mediator;

        private readonly Dictionary<string, ComandoRabbit> comandos;

        public WorkerRabbitFechamento(IServiceScopeFactory serviceScopeFactory,
                              ServicoTelemetria servicoTelemetria,
                              TelemetriaOptions telemetriaOptions,
                              ConnectionFactory factory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException("Service Scope Factory não localizado");
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.telemetriaOptions = telemetriaOptions ?? throw new ArgumentNullException(nameof(telemetriaOptions));

            ////TODO: REVER
            var scope = serviceScopeFactory.CreateScope();
            this.mediator = scope.ServiceProvider.GetService<IMediator>();
            ////

            var conexaoRabbit = factory.CreateConnection();

            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, 10, false);

            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.Sgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.SgpDeadLetter, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.SgpLogs, ExchangeType.Direct, true, false);

            DeclararFilasSgp();

            comandos = new Dictionary<string, ComandoRabbit>();
            RegistrarUseCases();
        }

        private void DeclararFilasSgp()
        {
            DeclararFilasPorRota(typeof(RotasRabbitFechamento), ExchangeSgpRabbit.Sgp, ExchangeSgpRabbit.SgpDeadLetter);
        }

        private void DeclararFilasPorRota(Type tipoRotas, string exchange, string exchangeDeadLetter = "")
        {
            var args = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(exchangeDeadLetter))
                args.Add("x-dead-letter-exchange", exchangeDeadLetter);

            foreach (var fila in tipoRotas.ObterConstantesPublicas<string>())
            {
                canalRabbit.QueueDeclare(fila, true, false, false, args);
                canalRabbit.QueueBind(fila, exchange, fila, null);

                if (!string.IsNullOrEmpty(exchangeDeadLetter))
                {
                    var filaDeadLetter = $"{fila}.deadletter";
                    canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, null);
                    canalRabbit.QueueBind(filaDeadLetter, exchangeDeadLetter, fila, null);
                }
            }
        }

        private void RegistrarUseCases()
        {
            // Consolidação fechamento turmas
            comandos.Add(RotasRabbitFechamento.ConsolidarTurmaSync, new ComandoRabbit("Inicia processo de Consolidação Fechamento/Conselho - Consolidar Turmas", typeof(IExecutarConsolidacaoTurmaGeralUseCase)));
            comandos.Add(RotasRabbitFechamento.ConsolidarTurmaTratar, new ComandoRabbit("Consolidação Fechamento/Conselho - Consolidar Turmas", typeof(IExecutarConsolidacaoTurmaUseCase)));

            comandos.Add(RotasRabbitFechamento.ConsolidarTurmaConselhoClasseSync, new ComandoRabbit("Consolidação conselho classe - Sincronizar alunos da turma", typeof(IExecutarConsolidacaoTurmaConselhoClasseUseCase)));
            comandos.Add(RotasRabbitFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, new ComandoRabbit("Consolidação conselho classe - Consolidar aluno da turma", typeof(IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase)));

            comandos.Add(RotasRabbitFechamento.ConsolidarTurmaFechamentoSync, new ComandoRabbit("Consolidação fechamento - Sincronizar Componentes da Turma", typeof(IExecutarConsolidacaoTurmaFechamentoUseCase)));
            comandos.Add(RotasRabbitFechamento.ConsolidarTurmaFechamentoComponenteTratar, new ComandoRabbit("Consolidação fechamento - Consolidar Componentes da Turma", typeof(IExecutarConsolidacaoTurmaFechamentoComponenteUseCase)));

            comandos.Add(RotasRabbitFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase)));
            comandos.Add(RotasRabbitFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores por ue", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase)));
            comandos.Add(RotasRabbitFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores por turma", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase)));
            comandos.Add(RotasRabbitFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores por aluno", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase)));
        }


        private async Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;
            if (comandos.ContainsKey(rota))
            {
                var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                var comandoRabbit = comandos[rota];
                try
                {
                    if (telemetriaOptions.Apm)
                        Agent.Tracer.StartTransaction("TratarMensagem", "WorkerRabbitSGP");

                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        AtribuirContextoAplicacao(mensagemRabbit, scope);

                        var casoDeUso = scope.ServiceProvider.GetService(comandoRabbit.TipoCasoUso);

                        var metodo = ObterMetodo(comandoRabbit.TipoCasoUso, "Executar");
                        await servicoTelemetria.RegistrarAsync(async () =>
                            await metodo.InvokeAsync(casoDeUso, new object[] { mensagemRabbit }),
                                                    "RabbitMQ",
                                                    "TratarMensagem",
                                                    rota);

                        canalRabbit.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (NegocioException nex)
                {
                    canalRabbit.BasicAck(ea.DeliveryTag, false);

                    await RegistrarLog(ea, mensagemRabbit, nex, LogNivel.Negocio, $"Erros: {nex.Message}");
                    if (mensagemRabbit.NotificarErroUsuario)
                        NotificarErroUsuario(nex.Message, mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                }
                catch (ValidacaoException vex)
                {
                    canalRabbit.BasicAck(ea.DeliveryTag, false);

                    await RegistrarLog(ea, mensagemRabbit, vex, LogNivel.Negocio, $"Erros: {JsonConvert.SerializeObject(vex.Mensagens())}");

                    if (mensagemRabbit.NotificarErroUsuario)
                        NotificarErroUsuario($"Ocorreu um erro interno, por favor tente novamente", mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                }
                catch (Exception ex)
                {
                    canalRabbit.BasicReject(ea.DeliveryTag, false);
                    await RegistrarLog(ea, mensagemRabbit, ex, LogNivel.Critico, $"Erros: {ex.Message}");
                    if (mensagemRabbit.NotificarErroUsuario)
                        NotificarErroUsuario($"Ocorreu um erro interno, por favor tente novamente", mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                }
            }
            else
            {
                canalRabbit.BasicReject(ea.DeliveryTag, false);
                var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                await RegistrarLog(ea, mensagemRabbit, null, LogNivel.Critico, $"Rota não registrada");
            }
        }

        private async Task RegistrarLog(BasicDeliverEventArgs ea, MensagemRabbit mensagemRabbit, Exception ex, LogNivel logNivel, string observacao)
        {
            var mensagem = $"{mensagemRabbit.UsuarioLogadoRF} - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)} - ERRO - {ea.RoutingKey}";

            await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, logNivel, LogContexto.WorkerFechamento, observacao, rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
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

                canalRabbit.BasicPublish(ExchangeSgpRabbit.Sgp, RotasRabbitSgp.RotaNotificacaoUsuario, null, body);
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
                    _ = await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao tratar mensagem {ea.DeliveryTag}", LogNivel.Critico, LogContexto.WorkerRabbit, ex.Message));
                    canalRabbit.BasicReject(ea.DeliveryTag, false);
                }
            };

            RegistrarConsumerSgp(consumer);
            return Task.CompletedTask;
        }

        private void RegistrarConsumerSgp(EventingBasicConsumer consumer)
        {
            foreach (var fila in typeof(RotasRabbitFechamento).ObterConstantesPublicas<string>())
                canalRabbit.BasicConsume(fila, false, consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            canalRabbit.Close();
            conexaoRabbit.Close();
            return Task.CompletedTask;
        }
    }
}
