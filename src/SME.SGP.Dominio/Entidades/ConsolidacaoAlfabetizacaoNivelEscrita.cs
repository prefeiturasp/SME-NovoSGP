namespace SME.SGP.Dominio.Entidades
{
    public class ConsolidacaoAlfabetizacaoNivelEscrita
    {
        public long Id { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public short AnoLetivo { get; set; }
        public short Periodo { get; set; }
        public string NivelEscrita { get; set; }
        public int Quantidade { get; set; }
    }
}
