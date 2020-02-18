using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FechamentoTurmaDisciplinaBimestreDto
    {
        public int TotalAulasDadas { get; set; }
        public int TotalAulasPrevistas { get; set; }
        public int Bimestre { get; set; }
        public List<NotaConceitoAlunoBimestreDto> Alunos { get; set; }
    }
}
