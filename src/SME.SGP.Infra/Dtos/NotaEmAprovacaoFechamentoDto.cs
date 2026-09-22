namespace SME.SGP.Infra
{
    public class NotaEmAprovacaoFechamentoDto
    {
        public string CodigoAluno { get; set; }
        public long TurmaFechamentoId { get; set; }
        public long DisciplinaId { get; set; }
        public double Nota { get; set; }
    }
}
