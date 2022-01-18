using System;

namespace SME.SGP.Infra
{
    public  class AulaFrequenciaDto
    {
        public AulaFrequenciaDto(long aulaId, DateTime dataAula, int numeroAulas, bool ehReposicao, long? frequenciaId, bool aulaCj, bool desabilitado)
        {
            AulaId = aulaId;
            DataAula = dataAula;
            NumeroAulas = numeroAulas;
            FrequenciaId = frequenciaId;
            EhReposicao = ehReposicao;
            AulaCj = aulaCj;
            PodeEditar = desabilitado;
        }

        public long AulaId { get; }
        public DateTime DataAula { get; }
        public int NumeroAulas { get; }
        public long? FrequenciaId { get; }
        public bool EhReposicao { get; }
        public bool AulaCj { get; }
        public bool PodeEditar { get; }
        public bool EhDataSelecionadaFutura => DataAula.Date > DateTime.Now.Date;
    }
}
