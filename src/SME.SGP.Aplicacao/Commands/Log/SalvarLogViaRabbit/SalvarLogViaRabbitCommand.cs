using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class SalvarLogViaRabbitCommand : IRequest<bool>
    {
        public SalvarLogViaRabbitCommand(string mensagem, LogNivel nivel, LogContexto contexto, string observacao = "")
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Contexto = contexto;
            Observacao = observacao;
        }

        public string Mensagem { get; set; }
        public LogNivel Nivel { get; set; }
        public LogContexto Contexto { get; set; }
        public string Observacao { get; set; }
    }

}
