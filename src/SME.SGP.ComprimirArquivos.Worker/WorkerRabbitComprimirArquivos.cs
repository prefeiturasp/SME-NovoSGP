using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Apm;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Auditoria.Worker.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class WorkerRabbitComprimirArquivos : IHostedService
    {
        private readonly IConnection conexaoRabbit;
        private readonly IModel canalRabbit;
        private readonly TelemetriaOptions telemetriaOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public WorkerRabbitComprimirArquivos(IConnectionFactory factory, IOptions<TelemetriaOptions> telemetriaOptions, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory), "Service Scope Factory não localizado");
            this.telemetriaOptions = telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));

            conexaoRabbit = factory.CreateConnection();
            canalRabbit = conexaoRabbit.CreateModel();

            canalRabbit.BasicQos(0, 1, false);

            canalRabbit.ExchangeDeclare(RotasRabbitOtimizarArquivos.ExchangeSgp, ExchangeType.Direct, true, false);
            canalRabbit.ExchangeDeclare(RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter, ExchangeType.Direct, true, false);

            DeclararFilas();
        }

        private void DeclararFilas()
        {
            var args = new Dictionary<string, object>();
            args.Add("x-dead-letter-exchange", RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter);

            canalRabbit.QueueDeclare(RotasRabbitOtimizarArquivos.OtimizarArquivoImagem, true, false, false, args);
            canalRabbit.QueueBind(RotasRabbitOtimizarArquivos.OtimizarArquivoImagem, RotasRabbitOtimizarArquivos.ExchangeSgp, RotasRabbitOtimizarArquivos.OtimizarArquivoImagem, null);
            
            canalRabbit.QueueDeclare(RotasRabbitOtimizarArquivos.OtimizarArquivoVideo, true, false, false, args);
            canalRabbit.QueueBind(RotasRabbitOtimizarArquivos.OtimizarArquivoVideo, RotasRabbitOtimizarArquivos.ExchangeSgp, RotasRabbitOtimizarArquivos.OtimizarArquivoVideo, null);

            var argsDlq = new Dictionary<string, object>
            {
                { "x-queue-mode", "lazy" },
                { "x-dead-letter-exchange", RotasRabbitOtimizarArquivos.ExchangeSgp },
                { "x-message-ttl", RotasRabbitOtimizarArquivos.DeadLetterTTL }
            };

            var filaDeadLetterImagem = $"{RotasRabbitOtimizarArquivos.OtimizarArquivoImagem}.deadletter";
            var filaDeadLetterVideo = $"{RotasRabbitOtimizarArquivos.OtimizarArquivoVideo}.deadletter";

            canalRabbit.QueueDeclare(filaDeadLetterImagem, true, false, false, argsDlq);
            canalRabbit.QueueBind(filaDeadLetterImagem, RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter, RotasRabbitOtimizarArquivos.OtimizarArquivoImagem, null);
            
            canalRabbit.QueueDeclare(filaDeadLetterVideo, true, false, false, argsDlq);
            canalRabbit.QueueBind(filaDeadLetterVideo, RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter, RotasRabbitOtimizarArquivos.OtimizarArquivoVideo, null);
            
            var argsLimbo = new Dictionary<string, object> { { "x-queue-mode", "lazy" } };

            var filaLimboImagem = $"{RotasRabbitOtimizarArquivos.OtimizarArquivoImagem}.limbo";
            var filaLimboVideo = $"{RotasRabbitOtimizarArquivos.OtimizarArquivoVideo}.limbo";

            canalRabbit.QueueDeclare(filaLimboImagem, true, false, false, argsLimbo);
            canalRabbit.QueueBind(filaLimboImagem, RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter, filaDeadLetterImagem);
            
            canalRabbit.QueueDeclare(filaLimboVideo, true, false, false, argsLimbo);
            canalRabbit.QueueBind(filaLimboVideo, RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter, filaDeadLetterImagem, null);
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
            canalRabbit.BasicConsume(RotasRabbitOtimizarArquivos.OtimizarArquivoImagem, false, consumer);
            canalRabbit.BasicConsume(RotasRabbitOtimizarArquivos.OtimizarArquivoVideo, false, consumer);
        }

        private async Task TratarMensagem(BasicDeliverEventArgs ea)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;

            var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
            
            var transacao = telemetriaOptions.Apm ? Agent.Tracer.StartTransaction(rota, "WorkerRabbitComprimirArquivos") : null;

            try
            {
                var useCase = ObterUseCases(rota);
                
                if (telemetriaOptions.Apm)
                    transacao.CaptureSpan("RegistrarComprimirArquivos", "RabbitMQ", () =>useCase.Executar(mensagemRabbit)).Wait();
                else
                    useCase.Executar(mensagemRabbit).Wait();

                canalRabbit.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                transacao?.CaptureException(ex);

                var rejeicoes = GetRetryCount(ea.BasicProperties);
                if (++rejeicoes >= RotasRabbitOtimizarArquivos.QuantidadeReprocessamentoDeadLetter)
                {
                    canalRabbit.BasicAck(ea.DeliveryTag, false);

                    var filaLimbo = $"{ea.RoutingKey}.limbo";
                    await PublicarMensagem(filaLimbo, mensagemRabbit,  RotasRabbitOtimizarArquivos.ExchangeSgpDeadLetter);
                }
                else canalRabbit.BasicReject(ea.DeliveryTag, false);
                
            }
            finally
            {
                transacao?.End();
            }
        }

        private IComprimirUseCase ObterUseCases(string rota)
        {
            var scope = serviceScopeFactory.CreateScope();
            
            return rota switch
            {
                RotasRabbitOtimizarArquivos.OtimizarArquivoImagem => scope.ServiceProvider
                    .GetService<IComprimirImagensUseCase>(),
                RotasRabbitOtimizarArquivos.OtimizarArquivoVideo => scope.ServiceProvider
                    .GetService<IComprimirVideoUseCase>(),
                _ => null
            };
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
        
        private Task PublicarMensagem(string rota, object request, string exchange = null)
        {
            var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var body = Encoding.UTF8.GetBytes(mensagem);
            
            var props = canalRabbit.CreateBasicProperties();
            
            props.Persistent = true;

            canalRabbit.BasicPublish(exchange, rota, true, props, body);

            return Task.CompletedTask;
        }
    }
}