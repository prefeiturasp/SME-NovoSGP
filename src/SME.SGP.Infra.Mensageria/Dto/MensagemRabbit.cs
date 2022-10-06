using Newtonsoft.Json;
using System;

namespace SME.SGP.Infra
{
    public class MensagemRabbit 
    {
        public MensagemRabbit(string action, object mensagem, Guid codigoCorrelacao, string usuarioLogadoRF, bool notificarErroUsuario = false, string perfilUsuario = null, string administrador = null)
        {
            Action = action;
            Mensagem = mensagem;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoRF = usuarioLogadoRF;
            PerfilUsuario = perfilUsuario;
            Administrador = administrador;
        }

        public MensagemRabbit(object mensagem, Guid codigoCorrelacao, string usuarioLogadoNomeCompleto, string usuarioLogadoRF, Guid? perfil, bool notificarErroUsuario = false, string administrador = null, string acao = null)
        {
            Mensagem = mensagem;
            CodigoCorrelacao = codigoCorrelacao;
            UsuarioLogadoNomeCompleto = usuarioLogadoNomeCompleto;
            UsuarioLogadoRF = usuarioLogadoRF;
            NotificarErroUsuario = notificarErroUsuario;
            PerfilUsuario = perfil?.ToString();
            Administrador = administrador;
            Action = acao;
        }

        public MensagemRabbit(object mensagem)
        {
            Mensagem = mensagem;
        }

        public MensagemRabbit()
        {

        }
        public string Action { get; set; }
        public object Mensagem { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string UsuarioLogadoNomeCompleto { get; set; }
        public string UsuarioLogadoRF { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public string PerfilUsuario { get; set; }
        public string Administrador { get; set; }

        public T ObterObjetoMensagem<T>() where T : class
        {
            return JsonConvert.DeserializeObject<T>(Mensagem.ToString());
        }
    }
}
