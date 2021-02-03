using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ItineranciaDto
    {
        public ItineranciaDto()
        {

        }
        public DateTime DataVisita { get; set; }
        public DateTime DataRetornoVerificacao { get; set; }
        public IEnumerable<ItineranciaObjetivoDto> ObjetivosVisita { get; set; }
        public IEnumerable<ItineranciaUeDto> Ues { get; set; }
        public IEnumerable<ItineranciaAlunoDto> Alunos { get; set; }
        public IEnumerable<ItineranciaQuestaoDto> Questoes { get; set; }
    }
}
