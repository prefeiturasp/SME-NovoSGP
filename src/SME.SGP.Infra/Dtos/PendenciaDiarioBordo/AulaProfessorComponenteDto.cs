namespace SME.SGP.Infra
{
    public class AulaProfessorComponenteDto
    {
        public string ProfessorRf { get; set; }
        public long PendenciaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AulaId { get; set; }
        public string DescricaoComponenteCurricular { get; set; }
        public int PeriodoEscolarId { get; set; }

        public AulaProfessorComponenteDto() { }
    }
}
