using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommand : IRequest<bool>
    {
        public PublicarFilaSgpCommand(string rota, object filtros, Guid? codigoCorrelacao = null, Usuario usuarioLogado = null, bool notificarErroUsuario = false)
        {
            Filtros = filtros;
            CodigoCorrelacao = codigoCorrelacao ?? Guid.NewGuid();
            NotificarErroUsuario = notificarErroUsuario;
            Usuario = usuarioLogado;
            Rota = rota;
        }

        public string Rota { get; set; }
        public object Filtros { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public Usuario Usuario { get; set; }
        public bool NotificarErroUsuario { get; set; }
    }
}
