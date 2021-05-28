using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class TrataDeadletterRabbitUseCase : ITrataDeadletterRabbitUseCase
    {
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public TrataDeadletterRabbitUseCase(IConfiguration configuration, IMediator mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
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
                    while (true)
                    {
                        var mensagemParaEnviar = _channel.BasicGet(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente + ".deadletter", true);
                        if (mensagemParaEnviar == null)
                            break;
                        else
                        {
                            var mensagem = Encoding.UTF8.GetString(mensagemParaEnviar.Body.Span.ToArray());
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente, mensagem, Guid.NewGuid(), null, false));
                        }
                    }
                }
            }

            return true;
        }
    }
}
