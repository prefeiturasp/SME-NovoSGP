namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurmaEvasao
    {
        public FiltroConsolidacaoFrequenciaTurmaEvasao(long turmaId, int mes)
        {
            TurmaId = turmaId;
            Mes = mes;
        }

        public long TurmaId { get; set; }
        public int Mes { get; set; }
    }
}
