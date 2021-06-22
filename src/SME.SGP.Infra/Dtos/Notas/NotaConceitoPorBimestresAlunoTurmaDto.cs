namespace SME.SGP.Infra
{
    public class NotaConceitoPorBimestresAlunoTurmaDto
    {
        public NotaConceitoPorBimestresAlunoTurmaDto(long ueId, long turmaId, string alunoCodigo, int[] bimestres)
        {
            UeId = ueId;
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            Bimestres = bimestres;
        }

        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public string  AlunoCodigo { get; set; }
        public int[] Bimestres { get; set; }
    }
}
