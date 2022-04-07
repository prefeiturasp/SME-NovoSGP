namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurmaPorDre
    {
        public FiltroConsolidacaoFrequenciaTurmaPorDre(int ano, long dreId, double percentualMinimo, double percentualMinimoInfantil)
        {
            Ano = ano;
            DreId = dreId;
            PercentualMinimo = percentualMinimo;
            PercentualMinimoInfantil = percentualMinimoInfantil;
        }

        public int Ano { get; }
        public long DreId { get;  }
        public double PercentualMinimo { get; }
        public double PercentualMinimoInfantil { get; }
    }
}
