using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PendenciaCargaAulas.ServicosFakes
{
    public class ObterPendenciasParaInserirAulasEDiasQueryHandlerFake:  IRequestHandler<ObterPendenciasParaInserirAulasEDiasQuery, IEnumerable<AulasDiasPendenciaDto>>
    {
        public async Task<IEnumerable<AulasDiasPendenciaDto>> Handle(ObterPendenciasParaInserirAulasEDiasQuery request, CancellationToken cancellationToken)
        {
            return new List<AulasDiasPendenciaDto>()
            {
                new AulasDiasPendenciaDto{PendenciaId = 1,QuantidadeAulas = 1,QuantidadeDias = 1},
            };
        }
    }
}