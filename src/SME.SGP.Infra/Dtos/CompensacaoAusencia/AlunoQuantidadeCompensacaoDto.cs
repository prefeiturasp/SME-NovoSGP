namespace SME.SGP.Infra
{
    public class AlunoQuantidadeCompensacaoDto
    {
        public AlunoQuantidadeCompensacaoDto(string codigoAluno, int quantidadeCompensar)
        {
            CodigoAluno = codigoAluno;
            QuantidadeCompensar = quantidadeCompensar;
        }

        public string CodigoAluno { get; set; }
        public int QuantidadeCompensar { get; set; }
    }
}
