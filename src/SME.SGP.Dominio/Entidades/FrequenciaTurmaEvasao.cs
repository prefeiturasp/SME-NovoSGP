namespace SME.SGP.Dominio
{
    public class FrequenciaTurmaEvasao
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public int? Mes { get; set; }
        public int QuantidadeAlunosAbaixo50Porcento { get; set; }
        public int QuantidadeAlunos0Porcento { get; set; }
    }
}
