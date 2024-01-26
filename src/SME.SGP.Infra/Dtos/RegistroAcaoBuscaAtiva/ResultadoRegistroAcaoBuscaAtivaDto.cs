namespace SME.SGP.Infra
{
    public class ResultadoRegistroAcaoBuscaAtivaDto
    {
        public ResultadoRegistroAcaoBuscaAtivaDto() {}

        public ResultadoRegistroAcaoBuscaAtivaDto(long id)
        {
            Id = id;
        }
        
        public long Id { get; set; }
        
        public AuditoriaDto Auditoria { get; set; }
    }
}
