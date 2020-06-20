using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterMeusDadosQuery : IRequest<MeusDadosDto>
    {
        public string Login { get; set; }

        public ObterMeusDadosQuery(string login)
        {
            Login = login;
        }
    }
}
