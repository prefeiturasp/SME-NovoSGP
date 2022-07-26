using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoMensageria : IServicoMensageria
    {
        private readonly IConexoesRabbitFilasSGP conexaoRabbit;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IAsyncPolicy policy;

        public ServicoMensageria(IConexoesRabbitFilasSGP conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry)
        {
            this.conexaoRabbit = conexaoRabbit ?? throw new ArgumentNullException(nameof(conexaoRabbit));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        public async Task<bool> Publicar(MensagemRabbit request, string rota, string exchange, string nomeAcao)
        {
            var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var body = Encoding.UTF8.GetBytes(mensagem);

            await servicoTelemetria.RegistrarAsync(() =>
                    policy.ExecuteAsync(() => PublicarMensagem(rota, body, exchange)),
                            "RabbitMQ", nomeAcao, rota);

            return true;
        }

        public Task<bool> Publicar<T>(T mensagem, string rota, string exchange, string nomeAcao)
            => Publicar(new MensagemRabbit(mensagem), rota, exchange, nomeAcao);

        private async Task PublicarMensagem(string rota, byte[] body, string exchange = null)
        {
            var _channel = conexaoRabbit.Get();
            try
            {
                var props = _channel.CreateBasicProperties();
                props.Persistent = true;

                _channel.BasicPublish(exchange, rota, true, props, body);
            }
            finally
            {
                conexaoRabbit.Return(_channel);
            }
        }
    }
}
