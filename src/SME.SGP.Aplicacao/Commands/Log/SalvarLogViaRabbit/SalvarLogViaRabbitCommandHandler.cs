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
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly ConfiguracaoRabbitLogOptions configuracaoRabbitOptions;
        private readonly IServicoTelemetria servicoTelemetria;

        public SalvarLogViaRabbitCommandHandler(ConfiguracaoRabbitLogOptions configuracaoRabbitOptions, IServicoTelemetria servicoTelemetria)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
            this.servicoTelemetria = servicoTelemetria ?? throw new System.ArgumentNullException(nameof(servicoTelemetria));
        }
        public async Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = JsonConvert.SerializeObject(new LogMensagem(request.Mensagem, request.Nivel.ToString(), request.Contexto.ToString(), request.Observacao, request.Projeto), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore

                });

                var body = Encoding.UTF8.GetBytes(mensagem);

                servicoTelemetria.Registrar(() => PublicarMensagem(body), "RabbitMQ", "Salvar Log Via Rabbit", RotasRabbitSgp.RotaLogs);

                return await Task.FromResult(true);
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        private void PublicarMensagem(byte[] body)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbitOptions.HostName,
                UserName = configuracaoRabbitOptions.UserName,
                Password = configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
            };

            using var conexaoRabbit = factory.CreateConnection();
            using IModel _channel = conexaoRabbit.CreateModel();

            var props = _channel.CreateBasicProperties();

            //TODO: Trocar a fila para direct;
            _channel.QueueBind(RotasRabbitSgp.RotaLogs, ExchangeSgpRabbit.SgpLogs, RotasRabbitSgp.RotaLogs);
            _channel.BasicPublish(ExchangeSgpRabbit.SgpLogs, RotasRabbitSgp.RotaLogs, props, body);
        }
    }
    public class LogMensagem
    {
        public LogMensagem(string mensagem, string nivel, string contexto, string observacao, string projeto)
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Contexto = contexto;
            Observacao = observacao;
            Projeto = projeto;
            DataHora = DateTime.Now;
        }

        public string Mensagem { get; set; }
        public string Nivel { get; set; }
        public string Contexto { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public DateTime DataHora { get; set; }

    }

}
