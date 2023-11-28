using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTurmaItinerarioEnsinoMedioQueryHandlerFake : IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>
    {
        private const int SERIE = 2;
        private const string NOME_SERIE = "Segunda série";

        public async Task<IEnumerable<TurmaItinerarioEnsinoMedioDto>> Handle(ObterTurmaItinerarioEnsinoMedioQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<TurmaItinerarioEnsinoMedioDto>()
            {
                new TurmaItinerarioEnsinoMedioDto()
                {
                    Serie = SERIE,
                    Nome = NOME_SERIE,
                }
            });
        }
    }
}
