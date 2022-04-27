namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaAlunoMensal
    {
        public FiltroConsolidacaoFrequenciaAlunoMensal(string turmaCodigo, int mes)
        {
            TurmaCodigo = turmaCodigo;
            Mes = mes;
        }

        public string TurmaCodigo { get; set; }
        public int Mes { get; set; }
    }
}
