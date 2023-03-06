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

        public WorkerRabbitAuditoria(IConnectionFactory factory, IOptions<TelemetriaOptions> telemetriaOptions, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory), "Service Scope Factory não localizado");
            this.telemetriaOptions = telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));

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
            canalRabbit.QueueBind(RotasRabbitAuditoria.PersistirAuditoriaDB, RotasRabbitAuditoria.ExchangeSgp, RotasRabbitAuditoria.PersistirAuditoriaDB, null);

            var filaDeadLetter = $"{RotasRabbitAuditoria.PersistirAuditoriaDB}.deadletter";

            canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, null);
            canalRabbit.QueueBind(filaDeadLetter, RotasRabbitAuditoria.ExchangeSgpDeadLetter, RotasRabbitAuditoria.PersistirAuditoriaDB, null);
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

        private Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;

            var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
            var transacao = telemetriaOptions.Apm ? Agent.Tracer.StartTransaction(rota, "WorkerRabbitAuditoria") : null;
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var registrarAuditoriaUseCase = scope.ServiceProvider.GetService<IRegistrarAuditoriaUseCase>();

                if (telemetriaOptions.Apm)
                    transacao.CaptureSpan("RegistrarAuditoriaDB", "RabbitMQ", () =>
                        registrarAuditoriaUseCase.Executar(mensagemRabbit)).Wait();
                else
                    registrarAuditoriaUseCase.Executar(mensagemRabbit).Wait();

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

            return Task.CompletedTask;
        }
    }
}