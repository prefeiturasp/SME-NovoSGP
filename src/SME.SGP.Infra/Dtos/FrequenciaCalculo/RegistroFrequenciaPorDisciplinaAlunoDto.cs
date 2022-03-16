using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaPorDisciplinaAlunoDto
    {
        public int Bimestre { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalRemotos { get; set; }
        public int TotalPresencas { get; set; }
        public string AlunoCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
    }
}