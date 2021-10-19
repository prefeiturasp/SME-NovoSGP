using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerServidorRelatoriosCommandHandler : IRequestHandler<PublicaFilaWorkerServidorRelatoriosCommand, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IServicoTelemetria servicoTelemetria;

        public PublicaFilaWorkerServidorRelatoriosCommandHandler(IConfiguration configuration, IServicoTelemetria servicoTelemetria)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
        }

        public Task<bool> Handle(PublicaFilaWorkerServidorRelatoriosCommand request, CancellationToken cancellationToken)
        {
            byte[] body = FormataBodyWorker(request);

            servicoTelemetria.Registrar(() => PublicaMensagem(request, body),
                         "RabbitMQ", "PublicaFilaWorkerServidorRelatorios", request.Fila);           

            return Task.FromResult(true);
        }

        private void PublicaMensagem(PublicaFilaWorkerServidorRelatoriosCommand request, byte[] body)
        {
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
                    var props = _channel.CreateBasicProperties();
                    props.Persistent = true;

                    _channel.BasicPublish(ExchangeSgpRabbit.ServidorRelatorios, request.Fila, props, body);
                }
            }
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
