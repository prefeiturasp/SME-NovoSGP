using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra.Utilitarios;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RotasAgendamentoTratarUseCase : IRotasAgendamentoTratarUseCase
    {
        private readonly IOptions<ConfiguracaoRabbitOptions> configuracaoRabbit;
        private readonly IMediator mediator;
        private readonly IAsyncPolicy policy;

        public RotasAgendamentoTratarUseCase(IOptions<ConfiguracaoRabbitOptions> configuracaoRabbit, IReadOnlyPolicyRegistry<string> registry, IMediator mediator)
        {
            this.configuracaoRabbit = configuracaoRabbit ?? throw new ArgumentNullException(nameof(configuracaoRabbit));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var fila = mensagemRabbit.Mensagem.ToString();

            var factory = new ConnectionFactory
            {
                Port = configuracaoRabbit.Value.Port,
                HostName = configuracaoRabbit.Value.HostName,
                UserName = configuracaoRabbit.Value.UserName,
                Password = configuracaoRabbit.Value.Password,
                VirtualHost = configuracaoRabbit.Value.VirtualHost
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
                        if (mensagemParaEnviar.EhNulo())
                            break;

                        var mensagem = Encoding.UTF8.GetString(mensagemParaEnviar.Body.ToArray());
                        var mensagemRabbit = JsonConvert.DeserializeObject<MensagemRabbit>(mensagem);
                        var mensagemAgendamento = mensagemRabbit.ObterObjetoMensagem<MensagemAgendamentoSyncDto>();

                        await mediator.Send(new PublicarFilaSgpCommand(mensagemAgendamento.Rota, mensagemAgendamento.Objeto, Guid.NewGuid(), null));
                    }
                }
            }
        }
    }
}
