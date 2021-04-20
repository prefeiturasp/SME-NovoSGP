using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerSgpCommandHandler : IRequestHandler<PublicaFilaWorkerSgpCommand, bool>
    {
        private readonly ConfiguracaoRabbitOptions configuracaoRabbitOptions;

        public PublicaFilaWorkerSgpCommandHandler(ConfiguracaoRabbitOptions configuracaoRabbitOptions)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new ArgumentNullException(nameof(configuracaoRabbitOptions));
        }

        public Task<bool> Handle(PublicaFilaWorkerSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(request.Filtros,
                                             request.CodigoCorrelacao,
                                             request.UsuarioLogadoNomeCompleto,
                                             request.UsuarioLogadoRF,
                                             request.PerfilUsuario,
                                             request.NotificarErroUsuario);

            var mensagemJson = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(mensagemJson);

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
                    _channel.BasicPublish(RotasRabbit.ExchangeSgp, request.NomeFila, null, body);
                }
            }


            return Task.FromResult(true);
        }
    }
}
