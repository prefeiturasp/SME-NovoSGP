using System;

namespace SME.SGP.Infra.Dtos
{
    public class PublicaFilaRelatoriosDto
    {
        public PublicaFilaRelatoriosDto(string fila, object filtros, string endpoint, Guid codigoCorrelacao, string codigoRfUsuario, bool notificarErroUsuario = false, string perfilUsuario = null)
        {
            Fila = fila;
            Filtros = filtros;
            Endpoint = endpoint;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoRF = codigoRfUsuario;
            PerfilUsuario = perfilUsuario;
        }

        public string Fila { get; set; }
        public object Filtros { get; set; }
        public string Endpoint { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public string UsuarioLogadoRF { get; }
        public string PerfilUsuario { get; }
    }
}
