using MediatR;
using System;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.VerificaSeUsuarioPossuiAbrangencia
{
    public class VerificaSeUsuarioPossuiAbrangenciaQuery : IRequest<bool>
    {
        public VerificaSeUsuarioPossuiAbrangenciaQuery(string usuarioRf, Guid perfilSelecionado)
        {
            UsuarioRf = usuarioRf;
            PerfilSelecionado = perfilSelecionado;
        }

        public string UsuarioRf { get; set; }

        public Guid PerfilSelecionado { get; set; }
    }
}