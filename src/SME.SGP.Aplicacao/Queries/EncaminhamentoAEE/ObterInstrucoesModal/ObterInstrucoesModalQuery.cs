using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterInstrucoesModalQuery : IRequest<string>
    {
        private static ObterInstrucoesModalQuery _instance;
        public static ObterInstrucoesModalQuery Instance => _instance ??= new();
    }
}
