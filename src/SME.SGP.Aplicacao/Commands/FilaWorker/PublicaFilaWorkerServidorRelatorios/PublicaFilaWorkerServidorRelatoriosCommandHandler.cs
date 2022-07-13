using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerServidorRelatoriosCommandHandler : IRequestHandler<PublicaFilaWorkerServidorRelatoriosCommand, bool>
    {
        private readonly IConexoesRabbitFilasSGP conexaoRabbit;
        private readonly IServicoTelemetria servicoTelemetria;

        public PublicaFilaWorkerServidorRelatoriosCommandHandler(IConexoesRabbitFilasSGP conexaoRabbit, IServicoTelemetria servicoTelemetria)
        {
            this.conexaoRabbit = conexaoRabbit ?? throw new ArgumentNullException(nameof(conexaoRabbit));
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
            var _channel = conexaoRabbit.Get();
            try
            {
                var props = _channel.CreateBasicProperties();
                props.Persistent = true;

                _channel.BasicPublish(ExchangeSgpRabbit.ServidorRelatorios, request.Fila, props, body);
            }
            finally
            {
                conexaoRabbit.Return(_channel);
            }
        }

        private static byte[] FormataBodyWorker(PublicaFilaWorkerServidorRelatoriosCommand request)
        {
            var mensagem = new MensagemRabbit(request.Endpoint, request.Mensagem, request.CodigoCorrelacao, request.UsuarioLogadoRF, request.NotificarErroUsuario, request.PerfilUsuario);

            var mensagemJson = JsonConvert.SerializeObject(mensagem, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return Encoding.UTF8.GetBytes(mensagemJson);
        }
    }
}
