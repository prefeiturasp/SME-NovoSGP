using System;

namespace SME.SGP.Infra.Dtos
{
    public class CalcularFrequenciaDto
    {
        public string[] CodigosAlunos { get; set; }
        public DateTime DataReferencia { get; set; }
        public string CodigoTurma { get; set; }
        public string CodigoComponenteCurricular { get; set; }
        public int Bimestre { get; set; }
    }
}
