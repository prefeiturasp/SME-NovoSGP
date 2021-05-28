namespace SME.SGP.Dominio
{
    public class FrequenciaPreDefinida
    {
        public FrequenciaPreDefinida()
        {
        }
        public FrequenciaPreDefinida(int id, long turmaId, string codigoAluno, long componenteCurricularId, TipoFrequencia tipoLancamentoFrequencia)
        {
            Id = id;
            TurmaId = turmaId;
            CodigoAluno = codigoAluno;
            ComponenteCurricularId = componenteCurricularId;
            TipoLancamentoFrequencia = tipoLancamentoFrequencia;
        }

        public int Id { get; set; }
        public long TurmaId { get; set; }
        public string CodigoAluno { get; set; }
        public long ComponenteCurricularId { get; set; }
        public TipoFrequencia TipoLancamentoFrequencia { get; set; }
    }
}
