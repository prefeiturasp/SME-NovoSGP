namespace SME.SGP.Infra
{
    public class ResultadoEncaminhamentoNAAPADto
    {
        public ResultadoEncaminhamentoNAAPADto() {}

        public ResultadoEncaminhamentoNAAPADto(long id)
        {
            Id = id;
        }
        
        public long Id { get; set; }
        
        public AuditoriaDto Auditoria { get; set; }
    }
}
