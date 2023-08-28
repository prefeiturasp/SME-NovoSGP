using MediatR;

namespace SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease
{
    public class ObterUltimaVersaoQuery : IRequest<string>
    {
        private static ObterUltimaVersaoQuery _instance;
        public static ObterUltimaVersaoQuery Instance => _instance ??= new();
    }
}
