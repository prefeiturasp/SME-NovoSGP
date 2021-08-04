using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.GoogleClassroom.Infra;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RabbitDeadletterSrTratarUseCase : IRabbitDeadletterSrTratarUseCase
    {
        private readonly IConfiguration configuration;
        private readonly IAsyncPolicy policy;

        public RabbitDeadletterSrTratarUseCase(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var fila = mensagemRabbit.Mensagem.ToString();

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("ConfiguracaoRabbit:HostName").Value,
                UserName = configuration.GetSection("ConfiguracaoRabbit:UserName").Value,
                Password = configuration.GetSection("ConfiguracaoRabbit:Password").Value,
                VirtualHost = configuration.GetSection("ConfiguracaoRabbit:Virtualhost").Value
            };

            await policy.ExecuteAsync(() => TratarMensagens(fila, factory));

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
                            await Task.Run(() => _channel.BasicPublish(ExchangeSrRabbit.Sgp, fila, null, mensagemParaEnviar.Body.ToArray()));
                        }                        
                    }
                }
            }
        }
    }
}
