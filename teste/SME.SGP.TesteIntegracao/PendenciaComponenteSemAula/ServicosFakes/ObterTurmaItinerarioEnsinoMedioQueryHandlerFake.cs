using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaComponenteSemAula.ServicosFakes
{
    public class ObterTurmaItinerarioEnsinoMedioQueryHandlerFake : IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>
    {
        public async Task<IEnumerable<TurmaItinerarioEnsinoMedioDto>> Handle(ObterTurmaItinerarioEnsinoMedioQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<TurmaItinerarioEnsinoMedioDto>
            {
                new TurmaItinerarioEnsinoMedioDto() { Id = 9, Serie = 2 }
            };

            return await Task.FromResult(retorno);
        }
    }
}
