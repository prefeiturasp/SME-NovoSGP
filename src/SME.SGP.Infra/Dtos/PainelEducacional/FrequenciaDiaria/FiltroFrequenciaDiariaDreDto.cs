namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroFrequenciaDiariaDreDto : FiltroPaginacaoDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; } = null;
        public string DataFrequencia { get; set; } = null;
    }
}
