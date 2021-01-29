using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class RegistroItineranciaAluno
    {
        public string CodigoAluno { get; set; }
        public long RegistroItineranciaId { get; set; }
        public IEnumerable<RegistroItineranciaAlunoQuestao>  AlunosQuestoes { get; set; }
    }
}