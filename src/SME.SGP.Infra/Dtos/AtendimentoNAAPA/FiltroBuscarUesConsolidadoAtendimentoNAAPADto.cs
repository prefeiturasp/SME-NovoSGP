namespace SME.SGP.Infra.Dtos
{
    public class FiltroBuscarUesConsolidadoAtendimentoNAAPADto
    {
        public FiltroBuscarUesConsolidadoAtendimentoNAAPADto(long ueId, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
    }
}