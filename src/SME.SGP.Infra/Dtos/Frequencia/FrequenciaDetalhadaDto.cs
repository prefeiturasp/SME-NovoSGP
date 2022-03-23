using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaDetalhadaDto
    {
        public FrequenciaDetalhadaDto()
        {
            Aulas = new List<FrequenciaDetalhadaAulasDto>();
        }
        public string CodigoAluno { get; set; }
        public DateTime DataAula { get; set; }
        public long AulaId { get; set; }
        public bool PossuiAnotacao { get; set; }
        public IList<FrequenciaDetalhadaAulasDto> Aulas { get; set; }
        public IndicativoFrequenciaDto IndicativoFrequencia { get; set; }
    }
}
