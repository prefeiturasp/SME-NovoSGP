using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaItinerarioEnsinoMedioQuery : IRequest<IEnumerable<TurmaItinerarioEnsinoMedioDto>>
    {
        public ObterTurmaItinerarioEnsinoMedioQuery()
        {}

        private static ObterTurmaItinerarioEnsinoMedioQuery _instance;
        public static ObterTurmaItinerarioEnsinoMedioQuery Instance => _instance ??= new();
    }     
}
