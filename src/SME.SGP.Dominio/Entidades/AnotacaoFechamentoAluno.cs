namespace SME.SGP.Dominio
{
    public class AnotacaoFechamentoAluno : EntidadeBase
    {
        public FechamentoAluno FechamentoAluno { get; set; }
        public long FechamentoAlunoId { get; set; }
        public string Anotacao { get; set; }
    }
}
