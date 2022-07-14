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
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly IConexoesRabbitFilasLog conexaoRabbit;
        private readonly IServicoTelemetria servicoTelemetria;

        public SalvarLogViaRabbitCommandHandler(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria)
        {
            this.conexaoRabbit = conexaoRabbit ?? throw new ArgumentNullException(nameof(conexaoRabbit));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
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
                                                                           request.Rastreamento,
                                                                           request.ExcecaoInterna), new JsonSerializerSettings
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
            var _channel = conexaoRabbit.Get();
            try
            {
                var props = _channel.CreateBasicProperties();

                _channel.BasicPublish(ExchangeSgpRabbit.SgpLogs, RotasRabbitLogs.RotaLogs, props, body);
            }
            finally
            {
                conexaoRabbit.Return(_channel);
            }        
        }
    }
    public class LogMensagem
    {
        public LogMensagem(string mensagem, string nivel, string contexto, string observacao, string projeto, string rastreamento, string excecaoInterna)
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Contexto = contexto;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
            ExcecaoInterna = excecaoInterna;
            DataHora = DateTime.Now;
        }

        public string Mensagem { get; set; }
        public string Nivel { get; set; }
        public string Contexto { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
        public string ExcecaoInterna { get; set; }
        public DateTime DataHora { get; set; }

    }

}
