using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoIdQuery : IRequest<long>
    {
        private static ObterUsuarioLogadoIdQuery _instance;
        public static ObterUsuarioLogadoIdQuery Instance => _instance ??= new();
    }
}
