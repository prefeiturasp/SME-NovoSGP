using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesBaseItineranciaEAlunoQuery : IRequest<ItineranciaQuestoesBaseDto>
    {
        private static ObterQuestoesBaseItineranciaEAlunoQuery _instance;
        public static ObterQuestoesBaseItineranciaEAlunoQuery Instance => _instance ??= new();
    }
}
