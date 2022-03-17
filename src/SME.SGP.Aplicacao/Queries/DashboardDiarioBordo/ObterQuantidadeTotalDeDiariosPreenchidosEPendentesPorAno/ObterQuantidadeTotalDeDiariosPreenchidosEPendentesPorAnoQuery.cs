using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoQuery : IRequest<IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>>
    {
        public ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoQuery(int anoLetivo, long dreId, long ueId, Modalidade modalidade)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}
