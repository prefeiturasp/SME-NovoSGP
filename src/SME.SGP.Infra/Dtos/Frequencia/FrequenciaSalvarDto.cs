using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FrequenciaSalvarDto
    {
        public string AlunoCodigo { get; set; }
        public DateTime DataAula { get; set; }
        public long? AulaId { get; set; }
        public int? NumeroAula { get; set; }
        public TipoFrequencia? TipoFrequencia { get; set; }
        public List<FrequenciaAulasDoDiaDto> Aulas { get; set; }
    }
}
