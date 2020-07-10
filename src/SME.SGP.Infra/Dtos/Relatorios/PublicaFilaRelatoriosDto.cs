using System;

namespace SME.SGP.Infra.Dtos
{
    public class PublicaFilaRelatoriosDto
    {
        public PublicaFilaRelatoriosDto(string fila, object mensagem, string endpoint, Guid codigoCorrelacao, string codigoRfUsuario, bool notificarErroUsuario = false)
        {
            Fila = fila;
            Mensagem = mensagem;
            Endpoint = endpoint;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoRF = codigoRfUsuario;
        }

        public string Fila { get; set; }
        public object Mensagem { get; set; }
        public string Endpoint { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public string UsuarioLogadoRF { get; }
    }
}
