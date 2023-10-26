using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class PodePersistirTurmaDisciplinaQueryHandlerFake : IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>
    {
        public async Task<bool> Handle(PodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return !request.ComponenteParaVerificarAtribuicao.Equals("139");
        }
    }
}
