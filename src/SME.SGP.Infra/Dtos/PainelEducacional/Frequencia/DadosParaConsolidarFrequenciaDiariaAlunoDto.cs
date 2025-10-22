using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Frequencia
{
    public class DadosParaConsolidarFrequenciaDiariaAlunoDto
    {
        public string CodigoDre { get; set; }
        public int UeId { get; set; }
        public long TurmaId { get; set; }
        public string NomeTurma { get; set; }
        public int AnoLetivo { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalRemotos { get; set; }
        public int TotalAusentes { get; set; }
        public int TotalFrequencias { get; set; }
        public DateTime DataAula { get; set; }
    }
}
