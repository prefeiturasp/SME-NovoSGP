using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.GoogleClassroom.Infra;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommandHandler : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IAsyncPolicy policy;
        private readonly IMediator mediator;

        public PublicarFilaSgpCommandHandler(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry, IServicoTelemetria servicoTelemetria, IMediator mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            string usuarioLogadoNomeCompleto = command.Usuario?.Nome;
            string usuarioLogadoRf = command.Usuario?.CodigoRf;
            Guid? perfilUsuario = command.Usuario?.PerfilAtual;

            if(command.Usuario == null)
            {
                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                usuarioLogadoNomeCompleto = usuario.Nome;
                usuarioLogadoRf = usuario.CodigoRf;
                perfilUsuario = usuario.PerfilAtual;
            }

            var request = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             usuarioLogadoNomeCompleto,
                                             usuarioLogadoRf,
                                             perfilUsuario,
                                             command.NotificarErroUsuario);

            var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var body = Encoding.UTF8.GetBytes(mensagem);

            servicoTelemetria.Registrar(() => 
                    policy.ExecuteAsync(() => PublicarMensagem(command.Rota, body)), 
                            "RabbitMQ", "PublicarFilaSgp", command.Rota);

            return true;
        }

        private async Task PublicarMensagem(string rota, byte[] body)
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

                    _channel.BasicPublish(ExchangeSgpRabbit.Sgp, rota, props, body);
                }
            }
        }

    }
}
