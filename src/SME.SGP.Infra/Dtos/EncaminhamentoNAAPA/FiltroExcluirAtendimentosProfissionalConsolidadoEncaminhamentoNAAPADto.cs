namespace SME.SGP.Infra.Dtos
{
    public class FiltroExcluirAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto
    {
        public FiltroExcluirAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto(long ueId, int mes, int anoLetivo, string[] rfsProfissionaisIgnorados)
        {
            Mes = mes;
            AnoLetivo = anoLetivo;
            UeId = ueId;
            RfsProfissionaisIgnorados = rfsProfissionaisIgnorados;
        }

        public int Mes { get; set; }
        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public string [] RfsProfissionaisIgnorados { get; set; }
    }
}