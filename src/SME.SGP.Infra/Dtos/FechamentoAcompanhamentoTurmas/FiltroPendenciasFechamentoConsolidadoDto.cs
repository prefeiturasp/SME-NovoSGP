namespace SME.SGP.Infra
{
    public class FiltroPendenciasFechamentoConsolidadoDto
    {
        public FiltroPendenciasFechamentoConsolidadoDto(long turmaId, int bimestre, long componenteCurricularId)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}
