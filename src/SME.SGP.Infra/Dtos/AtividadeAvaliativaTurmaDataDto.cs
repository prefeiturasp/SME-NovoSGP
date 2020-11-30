using System;

namespace SME.SGP.Infra
{
    public class AtividadeAvaliativaTurmaDataDto
    {
        public DateTime DataAvaliacao { get; set; }
        public int TurmaId { get; set; }
        public string[] DisciplinasId { get; set; }
    }
}
