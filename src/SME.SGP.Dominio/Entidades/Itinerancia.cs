using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Itinerancia : EntidadeBase
    {
        public DateTime DataVisita { get; set; }
        public IEnumerable<ItineranciaObjetivo> ObjetivosVisita { get; set; }
        public IEnumerable<ItineranciaUe> Ues { get; set; }
        public IEnumerable<ItineranciaAluno> Alunos { get; set; }
        public DateTime DataRetornoVerificacao { get; set; }
        public IEnumerable<ItineranciaQuestao> Questoes { get; set; }

    }
}
