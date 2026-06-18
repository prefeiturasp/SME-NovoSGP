using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterExecutarManutencaoAulasInfantilQuery : IRequest<bool>
    {
        private static ObterExecutarManutencaoAulasInfantilQuery _instance;
        public static ObterExecutarManutencaoAulasInfantilQuery Instance => _instance ??= new();
    }
}
