using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ItineranciaAlunoDto
    {
        public long Id { get; set; }
        public string CodigoAluno { get; set; }
        public string Nome { get; set; }
        public IEnumerable<ItineranciaAlunoQuestaoDto> Questoes { get; set; }
    }
}