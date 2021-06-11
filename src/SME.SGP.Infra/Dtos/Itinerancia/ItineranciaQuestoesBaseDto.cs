using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class ItineranciaQuestoesBaseDto
    {
        public ItineranciaQuestoesBaseDto()
        {
            ItineranciaQuestao = new List<ItineranciaQuestaoDto>();
            ItineranciaAlunoQuestao = new List<ItineranciaAlunoQuestaoDto>();
        }

        public IEnumerable<ItineranciaQuestaoDto> ItineranciaQuestao { get; set; }
        public IEnumerable<ItineranciaAlunoQuestaoDto> ItineranciaAlunoQuestao { get; set; }
    }
}
