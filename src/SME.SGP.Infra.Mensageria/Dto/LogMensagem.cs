using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class LogMensagem
    {
        public LogMensagem(string mensagem, string nivel, string contexto, string observacao, string projeto, string rastreamento, string excecaoInterna,string innerException)
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Contexto = contexto;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
            ExcecaoInterna = excecaoInterna;
            DataHora = DateTimeExtension.HorarioBrasilia();
            InnerException = innerException;
        }
        public string Mensagem { get; set; }
        public string Nivel { get; set; }
        public string Contexto { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
        public string ExcecaoInterna { get; set; }
        public string InnerException { get; set; }
        public DateTime DataHora { get; set; }
    }
}