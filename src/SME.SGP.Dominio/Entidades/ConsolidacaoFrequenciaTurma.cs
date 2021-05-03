namespace SME.SGP.Dominio
{
    public class ConsolidacaoFrequenciaTurma
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }

        public long QuantidadeAcimaMinimoFrequencia { get; set; }
        public long QuantidadeAbaixoMinimoFrequencia { get; set; }
    }
}