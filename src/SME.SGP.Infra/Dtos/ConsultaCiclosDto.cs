using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConsultaCiclosDto
    {
        public IEnumerable<int> IdsTurmas { get; set; }
        public int IdTurmaSelecionada { get; set; }
    }
}