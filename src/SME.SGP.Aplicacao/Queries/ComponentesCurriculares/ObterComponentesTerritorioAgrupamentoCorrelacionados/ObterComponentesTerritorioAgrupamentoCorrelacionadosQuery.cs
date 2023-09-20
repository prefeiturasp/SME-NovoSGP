using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery(long[] codigosComponentesCurricularesAgrupamentoTerritorioSaber, DateTime? dataReferencia = null)
        {
            CodigosComponentesCurricularesAgrupamentoTerritorioSaber = codigosComponentesCurricularesAgrupamentoTerritorioSaber;
            DataReferencia = dataReferencia;
        }

        public long[] CodigosComponentesCurricularesAgrupamentoTerritorioSaber { get; set; }
        public DateTime? DataReferencia { get; set; }

    }
}
