namespace SME.SGP.Infra
{
    public class SecaoPAPDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string NomeComponente { get; set; }
        public int Ordem { get; set; }
        public long QuestionarioId { get; set; }
        public bool QuestoesObrigatorias { get; set; }
        public long? PAPSecaoId { get; set; }
        public bool Concluido { get; set; }
        public long? PAPTurmaId { get; set; }
        public long? PAPAlunoId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
