using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class RegistroItinerancia : EntidadeBase
    {
        public DateTime DataVisita { get; set; }
        public IEnumerable<RegistroItineranciaObjetivo> ObjetivosVisita { get; set; }
        public IEnumerable<RegistroItineranciaUe> Ues { get; set; }
        public IEnumerable<RegistroItineranciaAluno> Alunos { get; set; }
        public DateTime DataRetornoVerificacao { get; set; }
        public IEnumerable<RegistroItineranciaQuestao> MyProperty { get; set; }

    }
}
