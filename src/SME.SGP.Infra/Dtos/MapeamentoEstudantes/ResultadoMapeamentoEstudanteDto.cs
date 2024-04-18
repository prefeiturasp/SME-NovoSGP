namespace SME.SGP.Infra
{
    public class ResultadoMapeamentoEstudanteDto
    {
        public ResultadoMapeamentoEstudanteDto() {}

        public ResultadoMapeamentoEstudanteDto(long id)
        {
            Id = id;
        }
        
        public long Id { get; set; }
        
        public AuditoriaDto Auditoria { get; set; }
    }
}
