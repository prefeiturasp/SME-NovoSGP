using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommand : IRequest<bool>
    {
        public PublicarFilaSgpCommand(string rota, object filtros, Guid codigoCorrelacao, Usuario usuarioLogado, bool notificarErroUsuario = false)
        {
            Filtros = filtros;
            CodigoCorrelacao = codigoCorrelacao;
            NotificarErroUsuario = notificarErroUsuario;
            UsuarioLogadoNomeCompleto = usuarioLogado?.Nome;
            UsuarioLogadoRF = usuarioLogado?.CodigoRf;
            PerfilUsuario = usuarioLogado?.PerfilAtual;
            Rota = rota;
        }

        public string Rota { get; set; }
        public object Filtros { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string UsuarioLogadoNomeCompleto { get; set; }
        public string UsuarioLogadoRF { get; set; }
        public Guid? PerfilUsuario { get; set; }
        public bool NotificarErroUsuario { get; set; }
    }
}
