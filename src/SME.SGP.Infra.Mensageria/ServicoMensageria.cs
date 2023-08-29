using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.IO;
using System.Linq;
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

            if (!ValidarPublicacao(request))
                return true;
 
            Func<Task> fnTaskPublicarMensagem = async () => await PublicarMensagem(rota, body, exchange, canalRabbit);
            Func<Task> fnTaskPolicy = async () => await policy.ExecuteAsync(fnTaskPublicarMensagem);
            await servicoTelemetria.RegistrarAsync(fnTaskPolicy, "RabbitMQ", nomeAcao,
                                                    rota, ObterParametrosMensagem(request));
            return true;
        }

        protected virtual bool ValidarPublicacao(T request)
            => true;

        private Task PublicarMensagem(string rota, byte[] body, string exchange = null, IModel canalRabbit = null)
        {
            var channel = canalRabbit ?? conexaoRabbit.Get();
            try
            {
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                channel.BasicPublish(exchange, rota, true, props, body);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
            finally
            {
                conexaoRabbit.Return(channel);
            }
        }

        
        public virtual string ObterParametrosMensagem(T mensagemRabbit)
            => String.Empty;
    }

    public class ServicoMensageriaSGP : ServicoMensageria<MensagemRabbit>, IServicoMensageriaSGP
    {
        public ServicoMensageriaSGP(IConexoesRabbitFilasSGP conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) 
            : base(conexaoRabbit, servicoTelemetria, registry) { }
    }

    public class ServicoMensageriaLogs : ServicoMensageria<LogMensagem>, IServicoMensageriaLogs
    {
        public ServicoMensageriaLogs(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) 
            : base(conexaoRabbit, servicoTelemetria, registry) { }

        public override string ObterParametrosMensagem(LogMensagem mensagemLog)
        {
            var json = JsonConvert.SerializeObject(mensagemLog);
            var mensagem = JsonConvert.DeserializeObject<LogMensagem>(json);
            return $"{mensagem!.Mensagem}, ExcecaoInterna:{mensagem.ExcecaoInterna}";
        }

        protected override bool ValidarPublicacao(LogMensagem mensagem)
            => !(AmbienteTestes() && mensagem.Nivel != "Critico");

        private bool AmbienteTestes()
            => new string[] { "Homologacao", "Homologacao-R2", "Desenvolvimento", "Testes", "Development", "Treinamento" }
            .Contains(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
    }

    public class ServicoMensageriaMetricas : ServicoMensageria<MetricaMensageria>, IServicoMensageriaMetricas
    {
        public ServicoMensageriaMetricas(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria, IReadOnlyPolicyRegistry<string> registry) : base(conexaoRabbit, servicoTelemetria, registry)
        {
        }

        public Task Concluido(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Ack, rota);

        public Task Erro(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Rej, rota);

        public Task Obtido(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Get, rota);

        public Task Publicado(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Pub, rota);

        private Task PublicarMetrica(TipoAcaoMensageria tipoAcao, string rota)
            => Publicar(new MetricaMensageria(tipoAcao.ToString(), rota),
                        RotasRabbitLogs.RotaMetricas,
                        ExchangeSgpRabbit.QueueLogs,
                        "Publicar Metrica Mensageria");

    }
}
