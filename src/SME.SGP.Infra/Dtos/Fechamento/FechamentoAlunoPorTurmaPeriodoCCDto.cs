using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoAlunoPorTurmaPeriodoCCDto
    {
        public FechamentoAlunoPorTurmaPeriodoCCDto()
        {
            FechamentoNotas = new List<FechamentoNotaPorTurmaPeriodoCCDto>();
        }

        public string AlunoCodigo { get; set; }
        public List<FechamentoNotaPorTurmaPeriodoCCDto> FechamentoNotas { get; }        
    }
}