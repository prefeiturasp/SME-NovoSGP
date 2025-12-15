using MediatR;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.VerificaSeUsuarioPossuiAbrangencia
{
    public class VerificaSeUsuarioPossuiAbrangenciaQuery : IRequest<bool>
    {
        public VerificaSeUsuarioPossuiAbrangenciaQuery(string usuarioRf)
        {
            UsuarioRf = usuarioRf;
        }

        public string UsuarioRf { get; set; }
    }
}
