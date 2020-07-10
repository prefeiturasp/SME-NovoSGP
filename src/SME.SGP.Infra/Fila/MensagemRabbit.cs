using Newtonsoft.Json;
using System;

namespace SME.SGP.Infra
{
    public class MensagemRabbit
    {
        public MensagemRabbit(string action, object mensagem, Guid codigoCorrelacao, string usuarioLogadoRF, bool notificarErroUsuario = false, string perfilUsuario = null)
        {
            Action = action;
            Mensagem = mensagem;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoRF = usuarioLogadoRF;
            PerfilUsuario = perfilUsuario;
        }

        public MensagemRabbit(object mensagem, Guid codigoCorrelacao, string usuarioLogadoNomeCompleto, string usuarioLogadoRF, Guid perfil, bool notificarErroUsuario = false)
        {
            Mensagem = mensagem;
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
        public object Mensagem { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string UsuarioLogadoNomeCompleto { get; set; }
        public string UsuarioLogadoRF { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public string PerfilUsuario { get; set; }

        public T ObterObjetoMensagem<T>() where T : class
        {
            return JsonConvert.DeserializeObject<T>(Mensagem.ToString());
        }
    }
}
