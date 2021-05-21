namespace SME.SGP.Infra
{
    public class TurmaAcompanhamentoFechamentoRetornoDto
    {        
        public long TurmaId { get; set; }
        public string Nome { get => $"Turma {nome}"; set { nome = value; } }

        private string nome;
    }
}
