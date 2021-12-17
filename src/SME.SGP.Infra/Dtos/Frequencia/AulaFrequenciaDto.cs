using System;

namespace SME.SGP.Infra
{
    public  class AulaFrequenciaDto
    {
        public AulaFrequenciaDto(long aulaId, DateTime dataAula, int numeroAulas, long? frequenciaId)
        {
            AulaId = aulaId;
            DataAula = dataAula;
            NumeroAulas = numeroAulas;
            FrequenciaId = frequenciaId;
        }

        public long AulaId { get; }
        public DateTime DataAula { get; }
        public int NumeroAulas { get; }
        public long? FrequenciaId { get; }
    }
}
