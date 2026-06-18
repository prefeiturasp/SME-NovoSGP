namespace SME.SGP.Infra
{
    public class RegistrosIndividuaisPeriodoDto
    {
        public PaginacaoResultadoDto<RegistroIndividualDto> RegistrosIndividuais { get; set; }

        public bool PodeRealizarNovoRegistro { get; set; }
    }
}
