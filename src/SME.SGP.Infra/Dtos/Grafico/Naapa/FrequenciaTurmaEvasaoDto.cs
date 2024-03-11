using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class FrequenciaTurmaEvasaoDto
    {
        public long TotalEstudantes { get; set; }
        public IEnumerable<GraficoFrequenciaTurmaEvasaoDto> GraficosFrequencia { get; set; }
    }
}
