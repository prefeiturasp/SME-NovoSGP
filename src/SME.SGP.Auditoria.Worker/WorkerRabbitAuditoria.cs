using Elastic.Apm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Auditoria.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker
{
    public class WorkerRabbitAuditoria : IHostedService
    {
        private readonly IConnection conexaoRabbit;
        private readonly IModel canalRabbit;
        private readonly TelemetriaOptions telemetriaOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public WorkerRabbitAuditoria(IConnectionFactory factory, IOptions<TelemetriaOptions> telemetriaOptions,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ??
                                       throw new ArgumentNullException(nameof(serviceScopeFactory),
                                           "Service Scope Factory não localizado");
            this.telemetriaOptions =
                telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));

            conexaoRabbit = factory.CreateConnection();
            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, 10, false);

            canalRabbit.ExchangeDeclare(RotasRabbitAuditoria.ExchangeSgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(RotasRabbitAuditoria.ExchangeSgpDeadLetter, ExchangeType.Direct, true, false);

            DeclararFilas();
        }

        private void DeclararFilas()
        {
            var args = new Dictionary<string, object>();

            args.Add("x-dead-letter-exchange", RotasRabbitAuditoria.ExchangeSgpDeadLetter);

            canalRabbit.QueueDeclare(RotasRabbitAuditoria.PersistirAuditoriaDB, true, false, false, args);
            canalRabbit.QueueBind(RotasRabbitAuditoria.PersistirAuditoriaDB, RotasRabbitAuditoria.ExchangeSgp,
                RotasRabbitAuditoria.PersistirAuditoriaDB, null);

            var filaDeadLetter = $"{RotasRabbitAuditoria.PersistirAuditoriaDB}.deadletter";

            canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, null);
            canalRabbit.QueueBind(filaDeadLetter, RotasRabbitAuditoria.ExchangeSgpDeadLetter,
                RotasRabbitAuditoria.PersistirAuditoriaDB, null);
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            canalRabbit.Close();
            conexaoRabbit.Close();

            return Task.CompletedTask;
        }

        private void RegistrarConsumerSgp(EventingBasicConsumer consumer)
        {
            canalRabbit.BasicConsume(RotasRabbitAuditoria.PersistirAuditoriaDB, false, consumer);
        }

        private async Task TratarMensagem(BasicDeliverEventArgs basicDeliverEventArgs)
        {

            Func<BasicDeliverEventArgs, Task> fnTaskAuditoria = async (basicDeliverEventArg) =>
            {
                using var scope = serviceScopeFactory.CreateScope();

                var mensagem = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.Span);
                var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);

                var registrarAuditoriaUseCase = scope.ServiceProvider.GetService<IRegistrarAuditoriaUseCase>()!;
                await registrarAuditoriaUseCase.Executar(mensagemRabbit);

                var deliveryTag = basicDeliverEventArg.DeliveryTag;
                canalRabbit.BasicAck(deliveryTag, false);

                //tem que pensar no caso de como fica caso a auditoria tenha sido enviada para o elastic
                //e o rabbit nao conseguiu confirmar o recebimento da mensagem
                //talvez alguma estrategia de idempotencia para verificar se a mensagem ja foi auditada anteriormente
                //caso aconteca um reenfileiramento
            };

            Action<BasicDeliverEventArgs, Exception> fnTaskAuditoriaException = (basicDeliverEventArg, _) =>
            {
                var deliveryTag = basicDeliverEventArg.DeliveryTag;
                canalRabbit.BasicReject(deliveryTag, false);
            };

            ///talvez encapsular e execucao do apm para algo mais generico sem boilerplate (method around)
            var rota = basicDeliverEventArgs.RoutingKey;
            await Apm.RunConditionalityWithSingleTransactionSpanAsync(telemetriaOptions.Apm, rota,
                "WorkerRabbitAuditoria", "RegistrarAuditoriaDB", "RabbitMQ",
                basicDeliverEventArgs, fnTaskAuditoria, fnTaskAuditoriaException);
        }


        //talvez mover para alguma classe publica caso precise utilizar o apm novamente.
        private sealed class Apm
        {
            public static async Task RunConditionalityWithSingleTransactionSpanAsync<T1>(bool useApm, string transactionName,
                string transactionType, string spanName, string spanType, T1 t1, Func<T1, Task> handler,
                Action<T1, Exception> exceptionHandler)
            {
                if (useApm)
                {
                    var transaction = Agent.Tracer.StartTransaction(transactionName, transactionType);
                    try
                    {
                        await transaction.CaptureSpan(spanName, spanType, async () => await handler(t1));
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler(t1, ex);
                    }
                    finally
                    {
                        transaction.End();
                    }
                }
                else
                {
                    try
                    {
                        await handler(t1);
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler(t1, ex);
                    }
                }
            }
        }
    }
}