using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommandHandler : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IConfiguration configuration;

        public PublicarFilaSgpCommandHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            var request = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             command.UsuarioLogadoNomeCompleto,
                                             command.UsuarioLogadoRF,
                                             command.PerfilUsuario,
                                             command.NotificarErroUsuario);

            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("ConfiguracaoRabbit:HostName"),
                UserName = configuration.GetValue<string>("ConfiguracaoRabbit:UserName"),
                Password = configuration.GetValue<string>("ConfiguracaoRabbit:Password"),
                VirtualHost = configuration.GetValue<string>("ConfiguracaoRabbit:Virtualhost")
            };

            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    var body = Encoding.UTF8.GetBytes(mensagem);


                    _channel.BasicPublish(RotasRabbit.ExchangeSgp, command.NomeFila, null, body);
                }
            }                

            return Task.FromResult(true);
        }
    }
}
