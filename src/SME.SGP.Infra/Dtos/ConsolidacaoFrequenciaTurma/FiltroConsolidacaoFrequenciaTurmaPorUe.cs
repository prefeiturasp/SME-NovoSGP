namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurmaPorUe
    {
        public FiltroConsolidacaoFrequenciaTurmaPorUe(int ano, long ueId, double percentualMinimo, double percentualMinimoInfantil)
        {
            Ano = ano;
            UeId = ueId;
            PercentualMinimo = percentualMinimo;
            PercentualMinimoInfantil = percentualMinimoInfantil;
        }

        public int Ano { get; }
        public long UeId { get; set; }
        public double PercentualMinimo { get; }
        public double PercentualMinimoInfantil { get; }
    }
}
