using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaAcessoSondagemQuery : IRequest<bool>
    {
        public ObterUsuarioPossuiAbrangenciaAcessoSondagemQuery(string usuarioRF, Guid usuarioPerfil)
        {
            UsuarioRF = usuarioRF;
            UsuarioPerfil = usuarioPerfil;
        }

        public string UsuarioRF { get; set; }
        public Guid UsuarioPerfil { get; set; }
    }
}
