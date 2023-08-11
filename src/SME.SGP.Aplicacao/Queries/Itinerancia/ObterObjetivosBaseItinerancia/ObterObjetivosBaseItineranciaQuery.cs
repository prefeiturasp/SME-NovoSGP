using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosBaseItineranciaQuery : IRequest<IEnumerable<ItineranciaObjetivosBaseDto>>
    {
        private static ObterObjetivosBaseItineranciaQuery _instance;
        public static ObterObjetivosBaseItineranciaQuery Instance => _instance ??= new();
    }
}
