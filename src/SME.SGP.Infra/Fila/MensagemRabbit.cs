using Newtonsoft.Json;
using System;

namespace SME.SGP.Infra
{
    public class MensagemRabbit
    {
        public MensagemRabbit(string action, object filtros, Guid codigoCorrelacao)
        {
            Action = action;
            Filtros = filtros;
            CodigoCorrelacao = codigoCorrelacao;
        }

        public string Action { get; set; }
        public object Filtros { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string UsuarioLogadoNomeCompleto { get; set; }
        public string UsuarioLogadoRF { get; set; }
        public T ObterObjetoFiltro<T>() where T : class
        {
            return JsonConvert.DeserializeObject<T>(Filtros.ToString());
        }
    }
}
