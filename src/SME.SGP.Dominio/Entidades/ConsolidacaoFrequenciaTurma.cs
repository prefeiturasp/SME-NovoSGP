namespace SME.SGP.Dominio
{
    public class ConsolidacaoFrequenciaTurma
    {
        public ConsolidacaoFrequenciaTurma() { }
        public ConsolidacaoFrequenciaTurma(long turmaId, int quantidadeAcimaMinimoFrequencia, int quantidadeAbaixoMinimoFrequencia)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }

        public int QuantidadeAcimaMinimoFrequencia { get; set; }
        public int QuantidadeAbaixoMinimoFrequencia { get; set; }
    }
}