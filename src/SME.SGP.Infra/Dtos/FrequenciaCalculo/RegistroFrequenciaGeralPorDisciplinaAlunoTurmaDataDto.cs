using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto
    {
        public string AlunoCodigo { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime DataAula { get; set; }
        public int TurmaId { get; set; }
    }
}