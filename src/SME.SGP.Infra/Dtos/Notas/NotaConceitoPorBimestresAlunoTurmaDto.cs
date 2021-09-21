namespace SME.SGP.Infra
{
    public class NotaConceitoPorBimestresAlunoTurmaDto
    {
        public NotaConceitoPorBimestresAlunoTurmaDto(string ueCodigo, string turmaCodigo, string alunoCodigo, int[] bimestres)
        {
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
            Bimestres = bimestres;
        }

        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string  AlunoCodigo { get; set; }
        public int[] Bimestres { get; set; }
    }
}
