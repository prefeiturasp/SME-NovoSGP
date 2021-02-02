using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RegistroItineranciaDto
    {
        public RegistroItineranciaDto()
        {

        }
        public DateTime DataVisita { get; set; }
        public DateTime DataRetornoVerificacao { get; set; }
        public IEnumerable<RegistroItineranciaObjetivoDto> ObjetivosVisita { get; set; }
        public IEnumerable<RegistroItineranciaUeDto> Ues { get; set; }
        public IEnumerable<RegistroItineranciaAlunoDto> Alunos { get; set; }
        public IEnumerable<RegistroItineranciaQuestaoDto> Questoes { get; set; }
    }
}
