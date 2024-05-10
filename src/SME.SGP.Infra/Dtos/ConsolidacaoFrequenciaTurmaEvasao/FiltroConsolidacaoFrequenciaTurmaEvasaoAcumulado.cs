namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado
    {
        public FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(int ano, long turmaId)
        {
            Ano = ano;
            TurmaId = turmaId;            
        }

        public int Ano { get; set; }
        public long TurmaId { get; set; }
        
    }
}
