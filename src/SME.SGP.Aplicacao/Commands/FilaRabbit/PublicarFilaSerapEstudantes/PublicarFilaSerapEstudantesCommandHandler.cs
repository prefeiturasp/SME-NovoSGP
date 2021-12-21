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
    public class PublicarFilaSerapEstudantesCommandHandler : IRequestHandler<PublicarFilaSerapEstudantesCommand, bool>
    {
        private readonly IConnection conexaoRabbit;

        public PublicarFilaSerapEstudantesCommandHandler(IConnection conexaoRabbit)
        {
            this.conexaoRabbit = conexaoRabbit ?? throw new System.ArgumentNullException(nameof(conexaoRabbit));
        }

        public Task<bool> Handle(PublicarFilaSerapEstudantesCommand request, CancellationToken cancellationToken)
        {
            using (IModel _channel = conexaoRabbit.CreateModel())
            {
                var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                byte[] body = FormataBodyWorker(request);

                _channel.BasicPublish(RotasRabbitSerapEstudantes.ExchangeSerapEstudantes, request.Fila, null, body);
            }

            return Task.FromResult(true);
        }

        private static byte[] FormataBodyWorker(PublicarFilaSerapEstudantesCommand request)
        {
            var mensagem = new MensagemRabbit(request.Mensagem);
            var mensagemJson = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(mensagemJson);
            return body;
        }
    }
}
