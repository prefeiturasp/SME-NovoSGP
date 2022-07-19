using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly IServicoMensageria servicoMensageria;

        public SalvarLogViaRabbitCommandHandler(IServicoMensageria servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new LogMensagem(request.Mensagem,
                                           request.Nivel.ToString(),
                                           request.Contexto.ToString(),
                                           request.Observacao,
                                           request.Projeto,
                                           request.Rastreamento,
                                           request.ExcecaoInterna);

            return servicoMensageria.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");
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
