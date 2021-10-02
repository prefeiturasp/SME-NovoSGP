using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly ConfiguracaoRabbitLogOptions configuracaoRabbitOptions;
        private readonly IServicoTelemetria servicoTelemetria;

        public SalvarLogViaRabbitCommandHandler(ConfiguracaoRabbitLogOptions configuracaoRabbitOptions, IServicoTelemetria servicoTelemetria)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
            this.servicoTelemetria = servicoTelemetria ?? throw new System.ArgumentNullException(nameof(servicoTelemetria));
        }
        public async Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                var body = Encoding.UTF8.GetBytes(mensagem);

                servicoTelemetria.Registrar(() => PublicarMensagem(body), "RabbitMQ", "Salvar Log Via Rabbit", RotasRabbitSgp.RotaLogs);

                return await Task.FromResult(true);
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        private void PublicarMensagem(byte[] body)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbitOptions.HostName,
                UserName = configuracaoRabbitOptions.UserName,
                Password = configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
            };

            using var conexaoRabbit = factory.CreateConnection();
            using IModel _channel = conexaoRabbit.CreateModel();

            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            _channel.BasicPublish(ExchangeSgpRabbit.SgpLogs, RotasRabbitSgp.RotaLogs, props, body);
        }
    }

}
