using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.GoogleClassroom.Infra;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaEmLoteSgpCommandHandler : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IAsyncPolicy policy;
        private readonly IMediator mediator;

        public PublicarFilaEmLoteSgpCommandHandler(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry, IMediator mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagens = new List<(string rota, byte[] body)>();

            foreach (var command in request.Commands)
            {
                string usuarioLogadoNomeCompleto = command.Usuario?.Nome;
                string usuarioLogadoRf = command.Usuario?.CodigoRf;
                Guid? perfilUsuario = command.Usuario?.PerfilAtual;

                if (command.Usuario == null)
                {
                    var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                    usuarioLogadoNomeCompleto = usuario.Nome;
                    usuarioLogadoRf = usuario.CodigoRf;
                    perfilUsuario = usuario.PerfilAtual;
                }

                var requisicao = new MensagemRabbit(command.Filtros,
                                                 command.CodigoCorrelacao,
                                                 usuarioLogadoNomeCompleto,
                                                 usuarioLogadoRf,
                                                 perfilUsuario,
                                                 command.NotificarErroUsuario);

                var mensagem = JsonConvert.SerializeObject(requisicao, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                var body = Encoding.UTF8.GetBytes(mensagem);

                mensagens.Add((command.Rota, body));
            }

            await policy.ExecuteAsync(() => PublicarMensagens(mensagens));

            return true;
        }

        private async Task PublicarMensagens(IEnumerable<(string rota, byte[] body)> mensagens)
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

                    foreach (var mensagem in mensagens)
                        _channel.BasicPublish(ExchangeSgpRabbit.Sgp, mensagem.rota, props, mensagem.body);                    
                }
            }
        }
    }
}
