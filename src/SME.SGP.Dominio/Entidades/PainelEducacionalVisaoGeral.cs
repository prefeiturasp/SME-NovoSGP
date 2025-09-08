namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalVisaoGeral : EntidadeBase
    {
        public string CodigoDre { get; set; }
        public int AnoLetivo { get; set; }
        public string Indicador { get; set; }
        public string Serie { get; set; }
        public decimal Valor { get; set; }
    }
}
