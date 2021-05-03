namespace SME.SGP.Dominio
{
    public class ConsolidacaoFrequenciaTurma : EntidadeBase
    {
        public Modalidade Modalidade { get; set; }
        public int AnoLetivo { get; set; }
        public int Semestre { get; set; }
        public string CodigoDre { get; set; }
        public string SiglaDre { get; set; }
        public string CodigoUe { get; set; }
        public string SiglaUe { get; set; }
        public string CodigoTurma { get; set; }

        public long QuantidadeAcimaMinimoFrequencia { get; set; }
        public long QuantidadeAbaixoMinimoFrequencia { get; set; }
    }
}