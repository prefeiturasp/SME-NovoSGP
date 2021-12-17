using System;

namespace SME.SGP.Infra
{
    public  class AulaFrequenciaDto
    {
        public AulaFrequenciaDto(long aulaId, DateTime dataAula, int numeroAulas, bool ehReposicao, long? frequenciaId)
        {
            AulaId = aulaId;
            DataAula = dataAula;
            NumeroAulas = numeroAulas;
            FrequenciaId = frequenciaId;
            EhReposicao = ehReposicao;
        }

        public long AulaId { get; }
        public DateTime DataAula { get; }
        public int NumeroAulas { get; }
        public long? FrequenciaId { get; }
        public bool EhReposicao { get; set; }
    }
}
