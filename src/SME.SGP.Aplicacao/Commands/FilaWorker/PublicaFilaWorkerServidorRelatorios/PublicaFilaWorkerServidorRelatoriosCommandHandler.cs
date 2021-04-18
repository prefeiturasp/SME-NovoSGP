using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Sentry;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerServidorRelatoriosCommandHandler : IRequestHandler<PublicaFilaWorkerServidorRelatoriosCommand, bool>
    {
        private readonly ConfiguracaoRabbitOptions configuracaoRabbitOptions;

        public PublicaFilaWorkerServidorRelatoriosCommandHandler(ConfiguracaoRabbitOptions configuracaoRabbitOptions)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
        }

        public Task<bool> Handle(PublicaFilaWorkerServidorRelatoriosCommand request, CancellationToken cancellationToken)
        {
            SentrySdk.CaptureMessage("3 - AdicionaFilaWorkerRelatorios");

            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbitOptions.HostName,
                UserName = configuracaoRabbitOptions.UserName,
                Password = configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
            };

            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    byte[] body = FormataBodyWorker(request);

                    _channel.BasicPublish(RotasRabbit.ExchangeGoogleSync, request.Fila, null, body);
                }
            }                 

            return Task.FromResult(true);
        }

        private static byte[] FormataBodyWorker(PublicaFilaWorkerServidorRelatoriosCommand request)
        {
            var mensagem = new MensagemRabbit(request.Endpoint, request.Mensagem, request.CodigoCorrelacao, request.UsuarioLogadoRF, request.NotificarErroUsuario, request.PerfilUsuario);
            var mensagemJson = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(mensagemJson);
            return body;
        }
    }
}
