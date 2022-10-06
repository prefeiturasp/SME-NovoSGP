using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public abstract class ServicoMensageria : IServicoMensageria
    {
        private readonly IConexoesRabbit conexaoRabbit;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IAsyncPolicy policy;

        public ServicoMensageria(IConexoesRabbit conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry)
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

            await servicoTelemetria.RegistrarAsync(async () =>
                    await policy.ExecuteAsync(async () => await PublicarMensagem(rota, body, exchange)),
                            "RabbitMQ", nomeAcao, rota,ObterParametrosMensagem(request));

            return true;
        }
        
        public Task<bool> Publicar<T>(T mensagem, string rota, string exchange, string nomeAcao)
            => Publicar(new MensagemRabbit(mensagem), rota, exchange, nomeAcao);

        private Task PublicarMensagem(string rota, byte[] body, string exchange = null)
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

            return Task.CompletedTask;
        }

        
        public virtual string ObterParametrosMensagem(MensagemRabbit mensagemRabbit)
        {
            return "";
        }
    }

    public class ServicoMensageriaSGP : ServicoMensageria, IServicoMensageriaSGP
    {
        public ServicoMensageriaSGP(IConexoesRabbitFilasSGP conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) 
            : base(conexaoRabbit, servicoTelemetria, registry) { }
    }

    public class ServicoMensageriaLogs : ServicoMensageria, IServicoMensageriaLogs
    {
        public override string ObterParametrosMensagem(MensagemRabbit mensagemRabbit)
        {
            var json = JsonConvert.SerializeObject(mensagemRabbit.Mensagem);
            var mensagem = JsonConvert.DeserializeObject<LogMensagem>(json);
            return mensagem!.Mensagem +", ExcecaoInterna:" + mensagem.ExcecaoInterna;
        }

        public ServicoMensageriaLogs(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) 
            : base(conexaoRabbit, servicoTelemetria, registry) { }
    }

}
