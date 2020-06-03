using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.PeriodoEscolar.PeriodoEscolarEmAberto
{
    public class PeriodoEscolarEmAbertoQuery: IRequest<bool>
    {
        public PeriodoEscolarEmAbertoQuery(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataReferencia = dataReferencia;
            Bimestre = bimestre;
            EhAnoLetivo = ehAnoLetivo;
        }

        public long TipoCalendarioId { get; set; }
        public DateTime DataReferencia { get; set; }
        public int Bimestre { get; set; }
        public bool EhAnoLetivo { get; set; }
    }
}
