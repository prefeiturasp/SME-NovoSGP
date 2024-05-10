using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDresQuery : IRequest<string[]>
    {
        public ObterCodigosDresQuery()
        {}

        private static ObterCodigosDresQuery _instance;
        public static ObterCodigosDresQuery Instance => _instance ??= new();
    }
}
