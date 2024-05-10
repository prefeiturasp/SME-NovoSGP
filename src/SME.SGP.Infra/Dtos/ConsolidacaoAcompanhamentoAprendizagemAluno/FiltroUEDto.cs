namespace SME.SGP.Infra
{
    public class FiltroUEDto
    {
        public FiltroUEDto(string ueCodigo, int anoLetivo = 0)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
        }

        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
    }
}
