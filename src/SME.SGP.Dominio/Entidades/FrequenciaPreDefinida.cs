namespace SME.SGP.Dominio
{
    public class FrequenciaPreDefinida
    {
        public int Id { get; set; }
        public long TurmaId { get; set; }
        public string CodigoAluno { get; set; }
        public long ComponenteCurricularId { get; set; }
        public SituacaoFrequencia Situacao { get; set; }
    }
}
