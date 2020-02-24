using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoTurmaDisciplinaBimestreDto
    {
        public int TotalAulasDadas { get; set; }
        public int TotalAulasPrevistas { get; set; }
        public int Bimestre { get; set; }
        public Periodo Periodo { get; set; }
        public List<NotaConceitoAlunoBimestreDto> Alunos { get; set; }
    }
}
