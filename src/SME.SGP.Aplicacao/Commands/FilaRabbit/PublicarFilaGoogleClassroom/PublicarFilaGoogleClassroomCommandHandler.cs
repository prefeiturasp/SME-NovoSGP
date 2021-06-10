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
    public class PublicarFilaGoogleClassroomCommandHandler : IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>
    {
        private readonly ConfiguracaoRabbitOptions configuracaoRabbitOptions;

        public PublicarFilaGoogleClassroomCommandHandler(ConfiguracaoRabbitOptions configuracaoRabbitOptions)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
        }

        public Task<bool> Handle(PublicarFilaGoogleClassroomCommand request, CancellationToken cancellationToken)
        {
            SentrySdk.CaptureMessage("3 - AdicionaFilaWorkerGoogleClassroom");

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

                    _channel.BasicPublish(RotasRabbitSgpGoogleClassroomApi.ExchangeGoogleSync, request.Fila, null, body);
                }
            }

            return Task.FromResult(true);
        }

        private static byte[] FormataBodyWorker(PublicarFilaGoogleClassroomCommand request)
        {
            var mensagem = new MensagemRabbit(request.Mensagem);
            var mensagemJson = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(mensagemJson);
            return body;
        }
    }
}
