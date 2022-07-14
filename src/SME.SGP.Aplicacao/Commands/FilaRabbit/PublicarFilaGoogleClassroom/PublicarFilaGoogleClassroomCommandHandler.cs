using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaGoogleClassroomCommandHandler : IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>
    {
        private readonly IConexoesRabbitFilasSGP conexaoRabbit;
        private readonly IServicoTelemetria servicoTelemetria;

        public PublicarFilaGoogleClassroomCommandHandler(IConexoesRabbitFilasSGP conexaoRabbit, IServicoTelemetria servicoTelemetria)
        {
            this.conexaoRabbit = conexaoRabbit ?? throw new System.ArgumentNullException(nameof(conexaoRabbit));
            this.servicoTelemetria = servicoTelemetria ?? throw new System.ArgumentNullException(nameof(servicoTelemetria));
        }

        public Task<bool> Handle(PublicarFilaGoogleClassroomCommand request, CancellationToken cancellationToken)
        {
            servicoTelemetria.Registrar(() => PublicarMensagem(request), "RabbitMQ", "Fila", request.Fila);            

            return Task.FromResult(true);
        }

        private void PublicarMensagem(PublicarFilaGoogleClassroomCommand request)
        {
            var _channel = conexaoRabbit.Get();
            try
            {
                var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                byte[] body = FormataBodyWorker(request);

                _channel.BasicPublish(RotasRabbitSgpGoogleClassroomApi.ExchangeGoogleSync, request.Fila, null, body);
            }
            finally
            {
                conexaoRabbit.Return(_channel);
            }
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
