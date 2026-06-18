using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTokenAtualQuery : IRequest<string>
    {
        private static ObterTokenAtualQuery _instance;
        public static ObterTokenAtualQuery Instance => _instance ??= new();
    }
}
