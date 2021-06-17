using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunosPorBimestreDto
    {
        public int Bimestre { get; set; }
        public int AulasPrevistas { get; set; }
        public int AulasDadas { get; set; }
        public bool MostraLabelAulasPrevistas { get; set; }
        public bool MostraColunaCompensacaoAusencia { get; set; }
        public IEnumerable<AlunoFrequenciaDto> FrequenciaAlunos { get; set; }
    }
}