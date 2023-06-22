using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterTipoNotaPorTurmaQueryHandlerFakeNota : IRequestHandler<ObterTipoNotaPorTurmaQuery, TipoNota>
    {
        public Task<TipoNota> Handle(ObterTipoNotaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(TipoNota.Nota);
        }
    }
}
