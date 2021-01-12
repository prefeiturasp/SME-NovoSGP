namespace SME.SGP.Infra
{
    public class ResultadoEncaminhamentoAEEDto
    {
        public long Id { get; set; }
        public long SecaoId { get; set; }
        public QuestaoAeeDto Questionario { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
