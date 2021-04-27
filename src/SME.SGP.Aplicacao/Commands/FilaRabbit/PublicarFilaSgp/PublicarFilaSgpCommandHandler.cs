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
    public class PublicarFilaSgpCommandHandler : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly ConfiguracaoRabbitOptions configuracaoRabbitOptions;

        public PublicarFilaSgpCommandHandler(ConfiguracaoRabbitOptions configuracaoRabbitOptions)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
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
                    var body = Encoding.UTF8.GetBytes(mensagem);


                    _channel.QueueBind(string.IsNullOrEmpty(command.Fila) ? RotasRabbit.FilaSgp : command.Fila, RotasRabbit.ExchangeSgp, command.NomeFila);
                    _channel.BasicPublish(RotasRabbit.ExchangeSgp, command.NomeFila, null, body);
                }
            }

            return Task.FromResult(true);
        }
    }
}
