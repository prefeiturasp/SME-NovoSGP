using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroFechamentoConsolidadoTurmaBimestreDto
    {
        public FiltroFechamentoConsolidadoTurmaBimestreDto(long turmaId, int bimestre, int situacaoFechamento)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            SituacaoFechamento = situacaoFechamento;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public int SituacaoFechamento { get; set; }

    }
}
