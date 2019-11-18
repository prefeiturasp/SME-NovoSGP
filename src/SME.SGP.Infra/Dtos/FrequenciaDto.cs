using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaDto
    {
        public FrequenciaDto(long aulaId)
        {
            AulaId = aulaId;
            ListaFrequencia = new List<RegistroFrequenciaAlunoDto>();
        }

        public long AulaId { get; set; }
        public IList<RegistroFrequenciaAlunoDto> ListaFrequencia { get; set; }
    }
}