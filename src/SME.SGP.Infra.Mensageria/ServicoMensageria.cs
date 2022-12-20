using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public abstract class ServicoMensageria<T> : IServicoMensageria<T>
        where T : class 
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

        public async Task<bool> Publicar(T request, string rota, string exchange, string nomeAcao, IModel canalRabbit = null)
        {
            var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var body = Encoding.UTF8.GetBytes(mensagem);

            await servicoTelemetria.RegistrarAsync(async () =>
                    await policy.ExecuteAsync(async () => await PublicarMensagem(rota, body, exchange, canalRabbit)),
                            "RabbitMQ", nomeAcao, rota, ObterParametrosMensagem(request));

            return true;
        }

        private Task PublicarMensagem(string rota, byte[] body, string exchange = null, IModel canalRabbit = null)
        {
            var channel = canalRabbit ?? conexaoRabbit.Get();
            try
            {
                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                channel.BasicPublish(exchange, rota, true, props, body);
            }
            finally
            {
                conexaoRabbit.Return(channel);
            }

            return Task.CompletedTask;
        }

        
        public virtual string ObterParametrosMensagem(T mensagemRabbit)
            => "";
    }

    public class ServicoMensageriaSGP : ServicoMensageria<MensagemRabbit>, IServicoMensageriaSGP
    {
        public ServicoMensageriaSGP(IConexoesRabbitFilasSGP conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) 
            : base(conexaoRabbit, servicoTelemetria, registry) { }
    }

    public class ServicoMensageriaLogs : ServicoMensageria<LogMensagem>, IServicoMensageriaLogs
    {
        public override string ObterParametrosMensagem(LogMensagem mensagemLog)
        {
            var json = JsonConvert.SerializeObject(mensagemLog);
            var mensagem = JsonConvert.DeserializeObject<LogMensagem>(json);
            return mensagem!.Mensagem +", ExcecaoInterna:" + mensagem.ExcecaoInterna;
        }

        public ServicoMensageriaLogs(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) 
            : base(conexaoRabbit, servicoTelemetria, registry) { }
    }

}
