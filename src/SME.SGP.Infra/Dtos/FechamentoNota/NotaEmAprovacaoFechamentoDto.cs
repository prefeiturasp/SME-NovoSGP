namespace SME.SGP.Infra
{
    public class NotaEmAprovacaoFechamentoDto
    {
        public long TurmaFechamentoId { get; set; }
        public long DisciplinaId { get; set; }
        public string CodigoAluno { get; set; }
        public double Nota { get; set; }
    }
}
