using System;

namespace SME.SGP.Infra
{
    public class AusenciaPorDisciplinaDto
    {
        public int Bimestre { get; set; }
        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public int TotalAusencias { get; set; }
        public string TurmaId { get; set; }
    }
}