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
        public Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = JsonConvert.SerializeObject(new LogMensagem(request.Mensagem,
                                                                           request.Nivel.ToString(),
                                                                           request.Contexto.ToString(),
                                                                           request.Observacao,
                                                                           request.Projeto,
                                                                           request.Rastreamento), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore

                });

                var body = Encoding.UTF8.GetBytes(mensagem);

                servicoTelemetria.Registrar(() => PublicarMensagem(body), "RabbitMQ", "Salvar Log Via Rabbit", RotasRabbitLogs.RotaLogs);

                return Task.FromResult(true);
            }
            catch (System.Exception)
            {
                return Task.FromResult(false);
            }
        }
        private void PublicarMensagem(byte[] body)
        {
            var factory = new ConnectionFactory
            {
                HostName =    configuracaoRabbitOptions.HostName,
                UserName =    configuracaoRabbitOptions.UserName,
                Password =    configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
            };

            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    var props = _channel.CreateBasicProperties();

                    _channel.BasicPublish(ExchangeSgpRabbit.SgpLogs, RotasRabbitLogs.RotaLogs, props, body);
                }                
            }            
        }
    }
    public class LogMensagem
    {
        public LogMensagem(string mensagem, string nivel, string contexto, string observacao, string projeto, string rastreamento)
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Contexto = contexto;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
            DataHora = DateTime.Now;
        }

        public string Mensagem { get; set; }
        public string Nivel { get; set; }
        public string Contexto { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
        public DateTime DataHora { get; set; }

    }

}
