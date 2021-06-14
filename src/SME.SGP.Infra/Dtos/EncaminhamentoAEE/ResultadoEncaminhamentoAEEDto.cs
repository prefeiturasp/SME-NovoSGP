namespace SME.SGP.Infra
{
    public class ResultadoEncaminhamentoAEEDto
    {
        public ResultadoEncaminhamentoAEEDto()
        {

        }

        public ResultadoEncaminhamentoAEEDto(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
