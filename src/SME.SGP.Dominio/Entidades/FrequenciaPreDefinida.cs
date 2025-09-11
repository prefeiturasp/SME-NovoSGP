namespace SME.SGP.Dominio
{
    public class FrequenciaPreDefinida
    {
        public FrequenciaPreDefinida()
        {
        }
        public FrequenciaPreDefinida(long id, long turmaId, string codigoAluno, long componenteCurricularId, TipoFrequencia tipoFrequencia)
        {
            Id = id;
            TurmaId = turmaId;
            CodigoAluno = codigoAluno;
            ComponenteCurricularId = componenteCurricularId;
            TipoFrequencia = tipoFrequencia;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }
        public string CodigoAluno { get; set; }
        public long ComponenteCurricularId { get; set; }
        public TipoFrequencia TipoFrequencia { get; set; }
    }
}
