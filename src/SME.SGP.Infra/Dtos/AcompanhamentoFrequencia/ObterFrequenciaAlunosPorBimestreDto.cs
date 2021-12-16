namespace SME.SGP.Infra
{
    public class ObterFrequenciaAlunosPorBimestreDto
    {
        public long ComponenteCurricularId { get; set; }
        public long TurmaId { get; set; }
        public int? Bimestre { get; set; }
        public bool PossuiTerritorio { get; set; } = false;
    }
}