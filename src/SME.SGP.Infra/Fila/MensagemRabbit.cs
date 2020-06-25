using Newtonsoft.Json;
using System;

namespace SME.SGP.Infra
{
    public class MensagemRabbit
    {
        public MensagemRabbit(string action, object filtros, Guid codigoCorrelacao, bool notificarErroUsuario = false)
        {
            Action = action;
            Filtros = filtros;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
        }

        public MensagemRabbit(object filtros, Guid codigoCorrelacao, string usuarioLogadoNomeCompleto, string usuarioLogadoRF, Guid perfil, bool notificarErroUsuario = false)
        {
            Filtros = filtros;
            CodigoCorrelacao = codigoCorrelacao;
            UsuarioLogadoNomeCompleto = usuarioLogadoNomeCompleto;
            UsuarioLogadoRF = usuarioLogadoRF;
            NotificarErroUsuario = notificarErroUsuario;
            PerfilUsuario = perfil.ToString();
        }

        protected MensagemRabbit()
        {

        }
        public string Action { get; set; }
        public object Filtros { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string UsuarioLogadoNomeCompleto { get; set; }
        public string UsuarioLogadoRF { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public string PerfilUsuario { get; set; }

        public T ObterObjetoFiltro<T>() where T : class
        {
            return JsonConvert.DeserializeObject<T>(Filtros.ToString());
        }
    }
}
