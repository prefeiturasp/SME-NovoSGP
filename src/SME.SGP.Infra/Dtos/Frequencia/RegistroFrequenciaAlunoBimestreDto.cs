namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoBimestreDto
    {
        public string CodigoAluno { get; set; }

        public int Bimestre { get; set; }

        public string CodigoTurma { get; set; }

        public long CodigoComponenteCurricular { get; set; }
    }
}
