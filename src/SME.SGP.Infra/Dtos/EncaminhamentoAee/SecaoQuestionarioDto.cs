namespace SME.SGP.Infra
{
    public class SecaoQuestionarioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool Concluido { get; set; }
        public long QuestionarioId { get; set; }
    }
}
