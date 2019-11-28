namespace SME.SGP.Dominio
{
    public class AtribuicaoCJ : EntidadeBase
    {
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string DreId { get; set; }
        public Modalidade Modalidade { get; set; }
        public string ProfessorRf { get; set; }
        public bool Substituir { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
    }
}