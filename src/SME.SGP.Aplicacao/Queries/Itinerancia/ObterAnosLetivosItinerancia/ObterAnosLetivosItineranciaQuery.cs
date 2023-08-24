using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosItineranciaQuery : IRequest<IEnumerable<int>>
    {
        private static ObterAnosLetivosItineranciaQuery _instance;
        public static ObterAnosLetivosItineranciaQuery Instance => _instance ??= new();
    }
}
