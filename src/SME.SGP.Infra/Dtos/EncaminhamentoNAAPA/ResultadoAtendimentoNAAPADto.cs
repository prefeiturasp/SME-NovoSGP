namespace SME.SGP.Infra
{
    public class ResultadoAtendimentoNAAPADto
    {
        public ResultadoAtendimentoNAAPADto() {}

        public ResultadoAtendimentoNAAPADto(long id)
        {
            Id = id;
        }
        
        public long Id { get; set; }
        
        public AuditoriaDto Auditoria { get; set; }
    }
}
