using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaSalvarAulaAlunosDto
    {
        public long AulaId { get; set; }
        public long? FrequenciaId { get; set; }
        public IEnumerable<FrequenciaSalvarAlunoDto> Alunos { get; set; }
    }
}
