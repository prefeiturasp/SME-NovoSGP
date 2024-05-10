namespace SME.SGP.Infra
{
    public class AulaPrevistaTurmaComponenteDto
    {
        public string ComponenteCurricularCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int AulasQuantidade { get; set; }
        public int Bimestre { get; set; }
        public long PeriodoEscolarId { get; set; }
    }
}
