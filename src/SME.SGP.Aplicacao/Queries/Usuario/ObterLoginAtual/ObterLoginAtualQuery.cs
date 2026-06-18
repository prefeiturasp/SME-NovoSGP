using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterLoginAtualQuery : IRequest<string>
    {
        private static ObterLoginAtualQuery _instance;
        public static ObterLoginAtualQuery Instance => _instance ??= new();
    }
}
