using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterPareceresConclusivosQueryHandlerFake : IRequestHandler<ObterPareceresConclusivosQuery, IEnumerable<ConselhoClasseParecerConclusivoDto>>
    {
        public ObterPareceresConclusivosQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivoDto>> Handle(ObterPareceresConclusivosQuery request, CancellationToken cancellationToken)
        {
            return new List<ConselhoClasseParecerConclusivoDto>();
        }
    }
}
