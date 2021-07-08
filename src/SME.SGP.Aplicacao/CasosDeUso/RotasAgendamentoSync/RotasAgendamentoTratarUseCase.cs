using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.GoogleClassroom.Infra;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RotasAgendamentoTratarUseCase : IRotasAgendamentoTratarUseCase
    {
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IAsyncPolicy policy;

        public RotasAgendamentoTratarUseCase(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry, IMediator mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

            return true;
        }

        private async Task TratarMensagens(string fila, ConnectionFactory factory)
        {
            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    while (true)
                    {
                        var mensagemParaEnviar = _channel.BasicGet(fila, true);
                        if (mensagemParaEnviar == null)
                            break;

                        var mensagem = Encoding.UTF8.GetString(mensagemParaEnviar.Body.ToArray());
                        var mensagemAgendamento = JsonConvert.DeserializeObject<MensagemAgendamentoSyncDto>(mensagem);

                        await mediator.Send(new PublicarFilaSgpCommand(mensagemAgendamento.Rota, mensagemAgendamento.Objeto, Guid.NewGuid(), null));
                    }
                }
            }
        }
    }
}
