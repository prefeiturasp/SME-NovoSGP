using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoQuery : IRequest<Usuario>
    {
        private static ObterUsuarioLogadoQuery _instance;
        public static ObterUsuarioLogadoQuery Instance => _instance ??= new();
    }
}
