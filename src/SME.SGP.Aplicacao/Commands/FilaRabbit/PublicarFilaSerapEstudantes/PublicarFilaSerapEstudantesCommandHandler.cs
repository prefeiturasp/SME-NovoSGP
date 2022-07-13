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
    public class PublicarFilaSerapEstudantesCommandHandler : IRequestHandler<PublicarFilaSerapEstudantesCommand, bool>
    {
        private readonly IConexoesRabbitFilasSGP conexaoRabbit;

        public PublicarFilaSerapEstudantesCommandHandler(IConexoesRabbitFilasSGP conexaoRabbit)
        {
            this.conexaoRabbit = conexaoRabbit ?? throw new System.ArgumentNullException(nameof(conexaoRabbit));
        }

        public Task<bool> Handle(PublicarFilaSerapEstudantesCommand request, CancellationToken cancellationToken)
        {
            var _channel = conexaoRabbit.Get();
            try
            {
                var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                byte[] body = FormataBodyWorker(request);

                _channel.BasicPublish(RotasRabbitSerapEstudantes.ExchangeSerapEstudantes, request.Fila, null, body);
            }
            finally
            {
                conexaoRabbit.Return(_channel);
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
