using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Dominio.Entidades
{
    public class Idep : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public SerieAnoIndiceDesenvolvimentoEnum SerieAno { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal Nota { get; set; }
    }
}