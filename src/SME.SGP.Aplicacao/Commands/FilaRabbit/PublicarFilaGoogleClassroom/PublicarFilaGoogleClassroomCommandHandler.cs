using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaGoogleClassroomCommandHandler : IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>
    {
        private readonly IModel rabbitChannel;

        public PublicarFilaGoogleClassroomCommandHandler(IModel rabbitChannel)
        {
            this.rabbitChannel = rabbitChannel ?? throw new ArgumentNullException(nameof(rabbitChannel));
        }

        public Task<bool> Handle(PublicarFilaGoogleClassroomCommand request, CancellationToken cancellationToken)
        {
            byte[] body = FormataBodyWorker(request);

            //rabbitChannel.QueueBind(request.Fila, RotasRabbit.ExchangeGoogleSync, RotasRabbit.FilaGoogleSync);
            //rabbitChannel.BasicPublish(RotasRabbit.ExchangeGoogleSync, request.Fila, null, body);

            SentrySdk.CaptureMessage("3 - AdicionaFilaWorkerGoogleClassroom");

            return Task.FromResult(true);
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
