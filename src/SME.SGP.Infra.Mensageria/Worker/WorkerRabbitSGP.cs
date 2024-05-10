using Elastic.Apm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Bindings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Excecoes;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Worker
{
    public abstract class WorkerRabbitSGP : IHostedService
    {
        protected  IModel canalRabbit;
        private IConnection conexaoRabbit;
        private IConnectionFactory factory;
        private IOptions<ConsumoFilasOptions> consumoFilasOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IServicoMensageriaSGP servicoMensageria;
        private readonly IServicoMensageriaMetricas servicoMensageriaMetricas;
        private readonly TelemetriaOptions telemetriaOptions;
        private readonly string apmTransactionType;
        private readonly Type tipoRotas;
        private readonly bool retryAutomatico;

        protected WorkerRabbitSGP(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory,
            string apmTransactionType,
            Type tipoRotas,
            bool retryAutomatico = true)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory), "Service Scope Factory não localizado");
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            this.servicoMensageriaMetricas = servicoMensageriaMetricas ?? throw new ArgumentNullException(nameof(servicoMensageriaMetricas));
            this.telemetriaOptions = telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.consumoFilasOptions = consumoFilasOptions ?? throw new ArgumentNullException(nameof(consumoFilasOptions));

            this.apmTransactionType = apmTransactionType ?? "WorkerRabbitSGP";
            this.tipoRotas = tipoRotas ?? throw new ArgumentNullException(nameof(tipoRotas));
            this.retryAutomatico = retryAutomatico;
            Comandos = new Dictionary<string, ComandoRabbit>();
            RegistrarUseCases();
            ConectarRabbitMQ();
        }

        protected Dictionary<string, ComandoRabbit> Comandos { get; }

        protected virtual void DeclararFilas()
        {
            DeclararFilasPorRota(ExchangeSgpRabbit.Sgp, ExchangeSgpRabbit.SgpDeadLetter);
        }

        protected abstract void RegistrarUseCases();

        protected void DeclararFilasPorRota(string exchange, string exchangeDeadLetter = "")
        {
            foreach (var fila in tipoRotas.ObterConstantesPublicas<string>())
            {
                var args = ObterArgumentoDaFila(fila, exchangeDeadLetter);
                canalRabbit.QueueDeclare(fila, true, false, false, args);
                canalRabbit.QueueBind(fila, exchange, fila, null);

                if (!string.IsNullOrEmpty(exchangeDeadLetter))
                {
                    var argsDlq = ObterArgumentoDaFilaDeadLetter(fila, exchange);
                    var filaDeadLetter = $"{fila}.deadletter";

                    canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, argsDlq);
                    canalRabbit.QueueBind(filaDeadLetter, exchangeDeadLetter, fila, null);

                    if (retryAutomatico)
                    {
                        var argsLimbo = new Dictionary<string, object>();
                        argsLimbo.Add("x-queue-mode", "lazy");

                        var queueLimbo = $"{fila}.limbo";
                        canalRabbit.QueueDeclare(
                            queue: queueLimbo,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: argsLimbo
                        );

                        canalRabbit.QueueBind(queueLimbo, exchangeDeadLetter, queueLimbo, null);
                    }
                }
            }
        }

        private Dictionary<string, object> ObterArgumentoDaFila(string fila, string exchangeDeadLetter)
        {
            var args = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(exchangeDeadLetter))
                args.Add("x-dead-letter-exchange", exchangeDeadLetter);

            if (Comandos.ContainsKey(fila) && Comandos[fila].ModeLazy)
                args.Add("x-queue-mode", "lazy");
            
            return args;
        }

        private Dictionary<string, object> ObterArgumentoDaFilaDeadLetter(string fila, string exchange)
        {
            var argsDlq = new Dictionary<string, object>();

            argsDlq.Add("x-queue-mode", "lazy");
            if (retryAutomatico)
            {
                var ttl = Comandos.ContainsKey(fila) ? Comandos[fila].TTL : ExchangeSgpRabbit.SgpDeadLetterTTL;
                argsDlq.Add("x-message-ttl", ttl);
                argsDlq.Add("x-dead-letter-exchange", exchange);
            }

            return argsDlq;
        }

        private ulong GetRetryCount(IBasicProperties properties)
        {
            if (properties.Headers.NaoEhNulo() && properties.Headers.ContainsKey("x-death"))
            {
                var deathProperties = (List<object>)properties.Headers["x-death"];
                var lastRetry = (Dictionary<string, object>)deathProperties[0];
                var count = lastRetry["count"];
                return (ulong) Convert.ToInt64(count);
            }
            else
            {
                return 0;
            }
        }

        public async Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;

            await servicoMensageriaMetricas.Obtido(rota);

            if (Comandos.ContainsKey(rota))
            {
                var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                var comandoRabbit = Comandos[rota];

                var transacao = telemetriaOptions.Apm ? Agent.Tracer.StartTransaction(rota, apmTransactionType) : null;
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    AtribuirContextoAplicacao(mensagemRabbit, scope);
                    IRabbitUseCase casoDeUso = (IRabbitUseCase)scope.ServiceProvider.GetService(comandoRabbit.TipoCasoUso);

                    await servicoTelemetria.RegistrarAsync(
                            async () => await casoDeUso.Executar(mensagemRabbit),
                            "RabbitMQ",
                            rota,
                            rota,
                            mensagem);

                    canalRabbit.BasicAck(ea.DeliveryTag, false);
                    await servicoMensageriaMetricas.Concluido(rota);
                }
                catch (NegocioException nex)
                {
                    transacao?.CaptureException(nex);

                    canalRabbit.BasicAck(ea.DeliveryTag, false);
                    await servicoMensageriaMetricas.Concluido(rota);

                    await RegistrarErroTratamentoMensagem(ea, mensagemRabbit, nex, LogNivel.Negocio, $"Erros: {nex.Message}");

                    if (mensagemRabbit.NotificarErroUsuario)
                        NotificarErroUsuario(nex.Message, mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                }
                catch (ValidacaoException vex)
                {
                    transacao?.CaptureException(vex);

                    canalRabbit.BasicAck(ea.DeliveryTag, false);
                    await servicoMensageriaMetricas.Concluido(rota);

                    await RegistrarErroTratamentoMensagem(ea, mensagemRabbit, vex, LogNivel.Negocio, $"Erros: {JsonConvert.SerializeObject(vex.Mensagens())}");

                    if (mensagemRabbit.NotificarErroUsuario)
                        NotificarErroUsuario($"Ocorreu um erro interno, por favor tente novamente", mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                }
                catch (Exception ex)
                {
                    transacao?.CaptureException(ex);

                    var rejeicoes = GetRetryCount(ea.BasicProperties);
                    if (++rejeicoes >= comandoRabbit.QuantidadeReprocessamentoDeadLetter)
                    {
                        canalRabbit.BasicAck(ea.DeliveryTag, false);

                        var filaLimbo = $"{ea.RoutingKey}.limbo";
                        await servicoMensageria.Publicar(mensagemRabbit, filaLimbo, ExchangeSgpRabbit.SgpDeadLetter, "PublicarDeadLetter");
                    }
                    else canalRabbit.BasicReject(ea.DeliveryTag, false);

                    await servicoMensageriaMetricas.Erro(rota);
                    await RegistrarErroTratamentoMensagem(ea, mensagemRabbit, ex, LogNivel.Critico, $"Erros: {ex.Message}");

                    if (mensagemRabbit.NotificarErroUsuario)
                        NotificarErroUsuario($"Ocorreu um erro interno, por favor tente novamente", mensagemRabbit.UsuarioLogadoRF, comandoRabbit.NomeProcesso);
                }
                finally
                {
                    transacao?.End();
                }
            }
            else
            {
                canalRabbit.BasicReject(ea.DeliveryTag, false);
                await servicoMensageriaMetricas.Erro(rota);

                var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                await RegistrarErroTratamentoMensagem(ea, mensagemRabbit, null, LogNivel.Critico, $"Rota não registrada");
            }
        }

        private void ConectarRabbitMQ()
        {
            conexaoRabbit = factory.CreateConnection();
            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, consumoFilasOptions.Value.Qos, false);

            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.Sgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.SgpDeadLetter, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.SgpLogs, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.QueueLogs, ExchangeType.Direct, true, false);
            DeclararFilas();
        }

        private void RegistrarConsumerSgp()
        {
            var consumer = new EventingBasicConsumer(canalRabbit);
            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    await TratarMensagem(ea);
                }
                catch (AlreadyClosedException ex)
                {
                    if (canalRabbit.IsClosed)
                        ReconectarRabbitMQ();
                    await RegistrarErro($"Erro Conexão RabbitMQ Fechada - {ea.RoutingKey}", LogNivel.Critico, $"{ex.Message} - {Encoding.UTF8.GetString(ea.Body.Span)}");
                }
                catch (Exception ex)
                {
                    await RegistrarErro($"Erro ao tratar mensagem {ea.DeliveryTag} - {ea.RoutingKey}", LogNivel.Critico, ex.Message);
                    canalRabbit.BasicReject(ea.DeliveryTag, false);
                }
            };

            RegistrarConsumerSgp(consumer, tipoRotas);
        }

        private void ReconectarRabbitMQ()
        {
            ConectarRabbitMQ();
            RegistrarConsumerSgp();
        }

        protected virtual Task RegistrarErroTratamentoMensagem(BasicDeliverEventArgs ea, MensagemRabbit mensagemRabbit, Exception ex, LogNivel logNivel, string observacao)
        {
            return Task.CompletedTask;
        }


        protected virtual Task RegistrarErro(string mensagem, LogNivel logNivel, string observacao = "", string rastreamento = "", string excecaoInterna = "")
        {
            return Task.CompletedTask;
        }

        protected virtual void AtribuirContextoAplicacao(MensagemRabbit mensagemRabbit, IServiceScope scope)
        {
        }

        protected virtual void NotificarErroUsuario(string message, string usuarioRf, string nomeProcesso)
        {
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            RegistrarConsumerSgp();
            return Task.CompletedTask;
        }

        private void RegistrarConsumerSgp(EventingBasicConsumer consumer, Type tipoRotas)
        {
            foreach (var fila in tipoRotas.ObterConstantesPublicas<string>())
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
