using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterOuAdicionarUsuarioIdCommand : IRequest<long>
    {
        public ObterOuAdicionarUsuarioIdCommand(string usuarioRF, string usuarioNome)
        {
            UsuarioRF = usuarioRF;
            UsuarioNome = usuarioNome;
        }

        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
    }
}
