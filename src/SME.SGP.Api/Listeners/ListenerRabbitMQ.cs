using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Api
{
    public class ListenerRabbitMQ : IHostedService
    {
        private readonly IModel canalRabbit;
        private readonly IServiceScopeFactory serviceScopeFactory;
        /// <summary>
        /// configuração da lista de tipos para a fila do rabbit instanciar, seguindo a ordem de propriedades:
        /// rota do rabbit, usaMediatr, tipo
        /// </summary>
        private readonly Dictionary<string, (bool, Type)> comandos;


        public ListenerRabbitMQ(IModel canalRabbit, IServiceScopeFactory serviceScopeFactory)
        {
            this.canalRabbit = canalRabbit ?? throw new ArgumentNullException(nameof(canalRabbit));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            canalRabbit.ExchangeDeclare(RotasRabbit.ExchangeListenerWorkerRelatorios, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbit.FilaListenerSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbit.FilaListenerSgp, RotasRabbit.ExchangeListenerWorkerRelatorios, "*", null);

            comandos = new Dictionary<string, (bool, Type)>();
            comandos.Add(RotasRabbit.RotaRelatoriosProntos, (false, typeof(IReceberRelatorioProntoUseCase)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(canalRabbit);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.Span;
                var content = System.Text.Encoding.UTF8.GetString(body);
                TratarMensagem(content, ea.RoutingKey);
                canalRabbit.BasicAck(ea.DeliveryTag, false);
            };

            //consumer.Shutdown += OnConsumerShutdown;
            //consumer.Registered += OnConsumerRegistered;
            //consumer.Unregistered += OnConsumerUnregistered;
            //consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            canalRabbit.BasicConsume(RotasRabbit.FilaListenerSgp, false, consumer);
            return Task.CompletedTask;
        }
        //relatorios.prontos
        private void TratarMensagem(string mensagem, string rota)
        {
            if (comandos.ContainsKey(rota))
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var tipoComando = comandos[rota];
                    var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);

                    if (tipoComando.Item1)
                    {
                    var comando = JsonConvert.DeserializeObject(mensagemRabbit.Filtros.ToString(), tipoComando.Item2);
                        var mediatr = scope.ServiceProvider.GetService<IMediator>();
                        mediatr.Send(comando);
                    }
                    else
                    {
                        var casoDeUsoConcreto = typeof(ICasoDeUso<>).MakeGenericType(tipoComando.Item2);

                        comando.GetMethod("Executar").Invoke(casoDeUso, new object[] { comando });
                    }

                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
