namespace SME.SGP.Infra
{
    public class FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto
    {
        public FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(long ueId, string turmaCodigo, string alunoCodigo, int[] bimestres, long componenteCurricularId)
        {
            UeId = ueId;
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
            Bimestres = bimestres;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long UeId { get; set; }
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public int[] Bimestres { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}
