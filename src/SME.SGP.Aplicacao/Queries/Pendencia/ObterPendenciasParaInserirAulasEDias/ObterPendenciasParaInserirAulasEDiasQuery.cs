using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaInserirAulasEDiasQuery : IRequest<IEnumerable<CargaAulasDiasPendenciaDto>>
    {
        public ObterPendenciasParaInserirAulasEDiasQuery(int? anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int? AnoLetivo { get; set; }
    }
}