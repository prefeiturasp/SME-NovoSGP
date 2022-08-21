using Elastic.Apm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Notificacao.Worker
{
    public class WorkerRabbitNotificacao : IHostedService
    {
        private readonly IConnection conexaoRabbit;
        private readonly IModel canalRabbit;
        private readonly TelemetriaOptions telemetriaOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public WorkerRabbitNotificacao(IConnectionFactory factory, IOptions<TelemetriaOptions> telemetriaOptions, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory), "Service Scope Factory não localizado");
            this.telemetriaOptions = telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));

            conexaoRabbit = factory.CreateConnection();
            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, 10, false);

            canalRabbit.ExchangeDeclare(RotasRabbitNotificacao.ExchangeSgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(RotasRabbitNotificacao.ExchangeSgpDeadLetter, ExchangeType.Direct, true, false);

            DeclararFilas();
        }

        private void DeclararFilas()
        {
            var args = new Dictionary<string, object>();

            args.Add("x-dead-letter-exchange", RotasRabbitNotificacao.ExchangeSgpDeadLetter);

            DeclararFila(RotasRabbitNotificacao.Criacao, args);
            DeclararFila(RotasRabbitNotificacao.Leitura, args);
            DeclararFila(RotasRabbitNotificacao.Exclusao, args);
        }

        private void DeclararFila(string fila, Dictionary<string, object> args)
        {
            canalRabbit.QueueDeclare(fila, true, false, false, args);
            canalRabbit.QueueBind(fila, RotasRabbitNotificacao.ExchangeSgp, fila, null);

            var filaDeadLetter = $"{fila}.deadletter";

            canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, null);
            canalRabbit.QueueBind(filaDeadLetter, RotasRabbitNotificacao.ExchangeSgpDeadLetter, fila, null);
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
                    canalRabbit.BasicReject(ea.DeliveryTag, false);
                }
            };

            RegistrarConsumerSgp(consumer);

            return Task.CompletedTask;
        }

        private async Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;

            MethodInfo[] methodsHub = typeof(NotificacaoHub).GetMethods();
            foreach (MethodInfo method in methodsHub)
            {
                ActionAttribute actionAttribute = GetActionAttribute(method);
                if (actionAttribute?.Rota == rota)
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetService(typeof(NotificacaoHub));
                    var notificacaoHub = (NotificacaoHub)service;

                    var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                    var transacao = telemetriaOptions.Apm ? Agent.Tracer.StartTransaction(rota, "WorkerRabbitNotificacao") : null;
                    try
                    {
                        if (telemetriaOptions.Apm)
                            await transacao.CaptureSpan(actionAttribute.Nome, "RabbitMQ", async () =>
                                await (Task)method.Invoke(notificacaoHub, new object[] { mensagemRabbit }));
                        else
                            await (Task)method.Invoke(notificacaoHub, new object[] { mensagemRabbit });

                        canalRabbit.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        transacao?.CaptureException(ex);

                        canalRabbit.BasicReject(ea.DeliveryTag, false);
                    }
                    finally
                    {
                        transacao?.End();
                    }

                }
            }
        }

        private ActionAttribute GetActionAttribute(MethodInfo method)
        {
            ActionAttribute actionAttribute = (ActionAttribute)method.GetCustomAttribute(typeof(ActionAttribute));
            return actionAttribute;
        }

        private void RegistrarConsumerSgp(EventingBasicConsumer consumer)
        {
            canalRabbit.BasicConsume(RotasRabbitNotificacao.Criacao, false, consumer);
            canalRabbit.BasicConsume(RotasRabbitNotificacao.Leitura, false, consumer);
            canalRabbit.BasicConsume(RotasRabbitNotificacao.Exclusao, false, consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            canalRabbit.Close();
            conexaoRabbit.Close();

            return Task.CompletedTask;
        }
    }
}
