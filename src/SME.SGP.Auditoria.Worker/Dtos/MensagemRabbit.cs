using Newtonsoft.Json;
using System;

namespace SME.SGP.Auditoria.Worker
{
    public class MensagemRabbit
    {
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
