namespace SME.SGP.Infra
{
    public class CalendarioTipoEventoPorDiaDto
    {
        public int Dia { get; set; }
        public int QuantidadeDeEventos { get; set; }
        public string[] TiposEvento { get; set; }
    }
}