namespace SME.SGP.Infra.Dtos
{
    public class FiltroBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto
    {
        public FiltroBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto(long ueId, int mes, int anoLetivo)
        {
            Mes = mes;
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }

        public int Mes { get; set; }
        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
    }
}