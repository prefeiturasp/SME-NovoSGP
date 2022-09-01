using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SME.SGP.Worker.RabbitMQ
{
    public class ConfigRabbit
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Queue { get; set; }    
        public string VirtualHost { get; set; }
    }
    public class ProcessMessageConsumer : IHostedService
    {
        private readonly IConnection _conn;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConfigRabbit _config;

        public ProcessMessageConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory), "Service Scope Factory não localizado");

            _config = new() { Host = "localhost", User = "user", Password = "bitnami", Queue = "JvsMain", VirtualHost = "dev" };
            this._serviceScopeFactory = serviceScopeFactory;

            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.User,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost
            };

            _conn = factory.CreateConnection();
            _channel = _conn.CreateModel();


            var exchange = "JvsMain";
            var exchangeDeadLetter = "JvsDeadLetter";
            _channel.ExchangeDeclare(exchange, ExchangeType.Direct, false, false);
            _channel.ExchangeDeclare(exchangeDeadLetter, ExchangeType.Direct, false, false);


            DeclararFila(_config.Queue, exchange, exchangeDeadLetter);
            DeclararFila("JvsTeste2", exchange, exchangeDeadLetter);

        }

        private void DeclararFila(string queue, string exchange, string exchangeDeadLetter)
        {
            var args = new Dictionary<string, object>();
            args.Add("x-dead-letter-exchange", exchangeDeadLetter);

            _channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args
            );
            _channel.QueueBind(queue, exchange, queue, null);

            var argsDlq = new Dictionary<string, object>();
            argsDlq.Add("x-dead-letter-exchange", exchange);
            argsDlq.Add("x-message-ttl", 10000);      

            var queueDlq = $"{queue}.deadletter";
            _channel.QueueDeclare(
                queue: queueDlq,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: argsDlq
            );

            _channel.QueueBind(queueDlq, exchangeDeadLetter, queue, null);


            var queueLimbu = $"{queue}.limbu";
            _channel.QueueDeclare(
                queue: queueLimbu,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.QueueBind(queueLimbu, exchangeDeadLetter, queueLimbu, null);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) => {
                try
                {
                    var contentArray = eventArgs.Body.ToArray();
                    var contentString = Encoding.UTF8.GetString(contentArray);

                    Debug.WriteLine(Convert.ToString(DateTime.Now)+" Consumindo");
                    Thread.Sleep(5000);
                    throw new Exception("Error " + contentString);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(Convert.ToString(DateTime.Now) + " Rejeitando");
                    if (eventArgs.DeliveryTag > 3)
                    {
                        _channel.BasicAck(eventArgs.DeliveryTag, false);
                        _channel.BasicPublish("JvsDeadLetter", "JvsMain.limbu", null, eventArgs.Body.ToArray());

                    }
                    else _channel.BasicReject(eventArgs.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(_config.Queue, false, consumer);
            _channel.BasicConsume("JvsTeste2", false, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


}
}
