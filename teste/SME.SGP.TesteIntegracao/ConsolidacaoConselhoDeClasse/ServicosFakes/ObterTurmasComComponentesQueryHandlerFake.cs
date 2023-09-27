using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes
{
    public class ObterTurmasComComponentesQueryHandlerFake : IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>
    {
        public ObterTurmasComComponentesQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<InfoComponenteCurricular>> Handle(ObterInfoComponentesCurricularesESPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
