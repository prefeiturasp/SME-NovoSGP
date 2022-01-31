using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class SalvarLogViaRabbitCommand : IRequest<bool>
    {
        public SalvarLogViaRabbitCommand(string mensagem, LogNivel nivel, LogContexto contexto, string observacao = "", string projeto = "SGP", string rastreamento = "")
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Contexto = contexto;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
        }

        public string Mensagem { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public LogNivel Nivel { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public LogContexto Contexto { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
    }

}
