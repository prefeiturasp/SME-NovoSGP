using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria
{

    public class FrequenciaDiariaDreDto
    {
        public List<RegistroFrequenciaDiariaUeDto> Ues { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}
