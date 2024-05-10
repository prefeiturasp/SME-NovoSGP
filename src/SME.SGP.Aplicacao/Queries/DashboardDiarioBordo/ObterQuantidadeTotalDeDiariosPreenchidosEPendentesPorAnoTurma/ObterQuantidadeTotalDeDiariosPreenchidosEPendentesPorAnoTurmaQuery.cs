using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery : IRequest<IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>>
    {
        public ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery(int anoLetivo, long dreId, long ueId, Modalidade modalidade, bool ehPerfilSMEDRE)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            EhPerfilSMEDRE = ehPerfilSMEDRE;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool EhPerfilSMEDRE { get; set; }
    }
}
