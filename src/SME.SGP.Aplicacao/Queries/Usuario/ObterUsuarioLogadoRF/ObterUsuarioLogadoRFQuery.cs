using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoRFQuery : IRequest<string>
    {
        private static ObterUsuarioLogadoRFQuery _instance;
        public static ObterUsuarioLogadoRFQuery Instance => _instance ??= new();
    }
}
