using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria
{
    public class FrequenciaDiariaUeDto
    {
        public List<RegistroFrequenciaDiariaTurmaDto> Turmas { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}
