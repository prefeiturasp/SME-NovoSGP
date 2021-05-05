namespace SME.SGP.Infra
{
    public class TurmaAcompanhamentoFechamentoRetornoDto
    {
        public long TurmaId { get; set; }
        public string Nome { get; set; }
        public string NomeFormatado
            => $"Turma {Nome}";
    }
}
