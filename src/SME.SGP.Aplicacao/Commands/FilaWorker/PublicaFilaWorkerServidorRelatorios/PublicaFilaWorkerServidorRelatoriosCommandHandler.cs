using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Sentry;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerServidorRelatoriosCommandHandler : IRequestHandler<PublicaFilaWorkerServidorRelatoriosCommand, bool>
    {
        private readonly IConfiguration configuration;

        public PublicaFilaWorkerServidorRelatoriosCommandHandler(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<bool> Handle(PublicaFilaWorkerServidorRelatoriosCommand request, CancellationToken cancellationToken)
        {
            byte[] body = FormataBodyWorker(request);

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("ConfiguracaoRabbit:HostName").Value,
                UserName = configuration.GetSection("ConfiguracaoRabbit:UserName").Value,
                Password = configuration.GetSection("ConfiguracaoRabbit:Password").Value,
                VirtualHost = configuration.GetSection("ConfiguracaoRabbit:Virtualhost").Value
            };

            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    _channel.BasicPublish(ExchangeRabbit.ServidorRelatorios, request.Fila, null, body);
                }
            }

            SentrySdk.CaptureMessage("3 - AdicionaFilaWorkerRelatorios");

            return Task.FromResult(true);
        }

        private static byte[] FormataBodyWorker(PublicaFilaWorkerServidorRelatoriosCommand request)
        {
            var mensagem = new MensagemRabbit(request.Endpoint, request.Mensagem, request.CodigoCorrelacao, request.UsuarioLogadoRF, request.NotificarErroUsuario, request.PerfilUsuario);
            var mensagemJson = JsonConvert.SerializeObject(mensagem, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var body = Encoding.UTF8.GetBytes(mensagemJson);
            return body;
        }
    }
}
