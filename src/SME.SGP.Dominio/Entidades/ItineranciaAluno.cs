using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class ItineranciaAluno : EntidadeBase
    {
        public string CodigoAluno { get; set; }
        public long RegistroItineranciaId { get; set; }
        public IEnumerable<ItineranciaAlunoQuestao>  AlunosQuestoes { get; set; }
        public bool Excluido { get; set; }
    }
}