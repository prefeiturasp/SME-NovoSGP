using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverPerfisUsuarioAtualCommand : IRequest
    {
        public RemoverPerfisUsuarioAtualCommand(string login)
        {
            Login = login;
        }

        public string Login { get; set; }
    }
}
