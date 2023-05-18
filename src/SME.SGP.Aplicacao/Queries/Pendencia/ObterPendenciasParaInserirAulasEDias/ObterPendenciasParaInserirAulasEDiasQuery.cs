using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaInserirAulasEDiasQuery : IRequest<IEnumerable<CargaAulasDiasPendenciaDto>>
    {
        public ObterPendenciasParaInserirAulasEDiasQuery(int? anoLetivo,long ueid)
        {
            AnoLetivo = anoLetivo;
            UeId = ueid;
        }

        public int? AnoLetivo { get; set; }
        public long UeId { get; set; }
    }
}