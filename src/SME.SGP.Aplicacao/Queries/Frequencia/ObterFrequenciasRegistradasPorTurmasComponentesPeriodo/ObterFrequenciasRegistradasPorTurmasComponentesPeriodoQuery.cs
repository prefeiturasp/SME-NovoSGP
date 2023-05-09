using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao

{
    public class ObterFrequenciasRegistradasPorTurmasComponentesPeriodoQuery : IRequest<IEnumerable<FrequenciaRegistradaDto>>
    {
        public ObterFrequenciasRegistradasPorTurmasComponentesPeriodoQuery(string codigosTurma, long[] componentesCurricularesId, IEnumerable<long> periodosEscolaresIds)
        {
            CodigosTurma = codigosTurma;
            ComponentesCurricularesId = componentesCurricularesId;
            PeriodosEscolaresId = periodosEscolaresIds;
        }
        public string CodigosTurma { get; set; }
        public long[] ComponentesCurricularesId { get; set; }
        public IEnumerable<long> PeriodosEscolaresId { get; set; }
    }
}