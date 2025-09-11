namespace SME.SGP.Dominio
{
    public class FrequenciaTurmaEvasaoAluno
    {
        public long Id { get; set; }
        public long FrequenciaTurmaEvasaoId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public double PercentualFrequencia { get; set; }
    }
}
