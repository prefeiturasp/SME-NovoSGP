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

namespace SME.SGP.Notificacoes.Worker
{
    public class WorkerRabbitNotificacao : IHostedService
    {
        private readonly IConnection conexaoRabbit;
        private readonly IModel canalRabbit;
        private readonly TelemetriaOptions telemetriaOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;

        private Dictionary<string, string> Comandos = new Dictionary<string, string>();

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
            RegistrarComandos();
        }

        private void RegistrarComandos()
        {
            Comandos.Add(RotasRabbitNotificacao.Criacao, "Criada");
            Comandos.Add(RotasRabbitNotificacao.Leitura, "Lida");
            Comandos.Add(RotasRabbitNotificacao.Exclusao, "Excluida");
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

            using var scope = serviceScopeFactory.CreateScope();
            var tipoHub = typeof(INotificacaoSgpHub);
            var hubNotificacoes = scope.ServiceProvider.GetService(tipoHub);

            var transacao = telemetriaOptions.Apm ? Agent.Tracer.StartTransaction(rota, "WorkerRabbitNotificacao") : null;
            if (Comandos.ContainsKey(rota))
            {
                try
                {
                    var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                    var comando = Comandos[rota];
                    var metodo = UtilMethod.ObterMetodo(tipoHub, comando);

                    if (telemetriaOptions.Apm)
                        await transacao.CaptureSpan(mensagemRabbit.Action, "RabbitMQ", async (span) =>
                        {
                            span.SetLabel("Mensagem", mensagem);
                            await (Task)metodo.Invoke(hubNotificacoes, new object[] { mensagemRabbit });
                        });
                    else
                        await (Task)metodo.Invoke(hubNotificacoes, new object[] { mensagemRabbit });

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
