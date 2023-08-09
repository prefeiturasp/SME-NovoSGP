using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RabbitDeadletterSgpTratarUseCase : IRabbitDeadletterSgpTratarUseCase
    {
        private readonly IConfiguration configuration;
        private readonly IAsyncPolicy policy;

        public RabbitDeadletterSgpTratarUseCase(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var fila = mensagemRabbit.Mensagem.ToString();

            var configuracaoRabbit = configuration.GetSection("ConfiguracaoRabbit");
            var connectionFactory = new ConnectionFactory
            {
                Port = configuracaoRabbit.GetValue<int>("Port"),
                HostName = configuracaoRabbit.GetValue<string>("HostName"),
                UserName = configuracaoRabbit.GetValue<string>("UserName"),
                Password = configuracaoRabbit.GetValue<string>("Password"),
                VirtualHost = configuracaoRabbit.GetValue<string>("Virtualhost")
            };

            await policy.ExecuteAsync(() => TratarMensagens(fila, connectionFactory));

            return await Task.FromResult(true);
        }

        private async Task TratarMensagens(string fila, ConnectionFactory factory)
        {
            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    while (true)
                    {
                        var mensagemParaEnviar = _channel.BasicGet($"{fila}.deadletter", true);                        

                        if (mensagemParaEnviar == null)
                            break;
                        else
                        {
                            //essa mensagem não é persistente ?
                            await Task.Run(() => _channel.BasicPublish(ExchangeSgpRabbit.Sgp, fila, null, mensagemParaEnviar.Body.ToArray()));
                        }                        
                    }
                }
            }
        }
    }
}
