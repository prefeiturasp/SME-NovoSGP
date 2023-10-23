using System;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoTurmaDto
    {
        public long RegistroFrequenciaAlunoId { get; set; }
        public DateTime DataAula { get; set; }
        public long AulaId { get; set; }
        public string DisciplinaCodigo { get; set; }
        public int Valor { get; set; }
    }
}
