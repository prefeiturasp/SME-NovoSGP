namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroFrequenciaDiariaUeDto : FiltroPaginacaoDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; } = null;
        public string DataFrequencia { get; set; } = null;
    }
}
