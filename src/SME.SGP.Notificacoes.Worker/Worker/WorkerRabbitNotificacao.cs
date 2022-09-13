using Elastic.Apm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
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
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IServiceScopeFactory serviceScopeFactory;

        private Dictionary<string, string> Comandos = new Dictionary<string, string>();

        public WorkerRabbitNotificacao(IConnectionFactory factory, IServicoTelemetria servicoTelemetria, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory), "Service Scope Factory não localizado");
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));

            conexaoRabbit = factory.CreateConnection();
            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, 10, false);

            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.Sgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(ExchangeSgpRabbit.SgpDeadLetter, ExchangeType.Direct, true, false);

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

            args.Add("x-dead-letter-exchange", ExchangeSgpRabbit.SgpDeadLetter);

            DeclararFila(RotasRabbitNotificacao.Criacao, args);
            DeclararFila(RotasRabbitNotificacao.Leitura, args);
            DeclararFila(RotasRabbitNotificacao.Exclusao, args);
        }

        private void DeclararFila(string fila, Dictionary<string, object> args)
        {
            canalRabbit.QueueDeclare(fila, true, false, false, args);
            canalRabbit.QueueBind(fila, ExchangeSgpRabbit.Sgp, fila, null);

            var filaDeadLetter = $"{fila}.deadletter";

            canalRabbit.QueueDeclare(filaDeadLetter, true, false, false, null);
            canalRabbit.QueueBind(filaDeadLetter, ExchangeSgpRabbit.SgpDeadLetter, fila, null);
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
                    Console.WriteLine($"*** ERRO: {ex.Message}");
                }
            };

            RegistrarConsumerSgp(consumer);

            return Task.CompletedTask;
        }

        private async Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;

            if (Comandos.ContainsKey(rota))
            {
                var transacao = servicoTelemetria.Iniciar(rota, "WorkerRabbitNotificacao");
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var tipoHub = typeof(INotificacaoSgpHub);
                    var hubNotificacoes = scope.ServiceProvider.GetService(tipoHub);

                    var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                    var comando = Comandos[rota];
                    var metodo = UtilMethod.ObterMetodo(tipoHub, comando);

                    await servicoTelemetria.RegistrarAsync(
                        async () => await (Task)metodo.Invoke(hubNotificacoes, new object[] { mensagemRabbit }), 
                        "RabbitMQ", 
                        rota, 
                        comando, 
                        mensagem);

                    canalRabbit.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    transacao?.CaptureException(ex);

                    throw;
                }
                finally
                {
                    servicoTelemetria.Finalizar(transacao);
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
