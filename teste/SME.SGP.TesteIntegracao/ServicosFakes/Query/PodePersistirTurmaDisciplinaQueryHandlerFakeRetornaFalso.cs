using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes.Query
{
    public class PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaFalso : IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>
    {
        public async Task<bool> Handle(PodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return false;
        }
    }
}
