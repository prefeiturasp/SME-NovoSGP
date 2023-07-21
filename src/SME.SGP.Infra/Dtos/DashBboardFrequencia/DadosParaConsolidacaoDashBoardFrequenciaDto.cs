using System;

namespace SME.SGP.Infra
{
    public class DadosParaConsolidacaoDashBoardFrequenciaDto
    {
        public int Presentes { get; set; }
        public int Remotos { get; set; }
        public int Ausentes { get; set; }
        public DateTime DataAula { get; set; }
        public string CodigoAluno { get; set; }
    }
}
