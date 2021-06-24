namespace SME.SGP.Infra
{
    public class FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto
    {
        public FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(string turmaCodigo, string alunoCodigo, int[] bimestres, string componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
            Bimestres = bimestres;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public int[] Bimestres { get; set; }
        public string ComponenteCurricularId { get; set; }
    }
}
