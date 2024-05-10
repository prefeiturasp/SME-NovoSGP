using System;

namespace SME.SGP.Infra
{
    public class AulaFrequenciaDto
    {
        public AulaFrequenciaDto(long aulaId, DateTime dataAula, int numeroAulas, bool ehReposicao, long? frequenciaId, bool aulaCj, bool desabilitado)
        {
            AulaId = aulaId;
            Data = dataAula;
            NumeroAulas = numeroAulas;
            FrequenciaId = frequenciaId;
            EhReposicao = ehReposicao;
            AulaCj = aulaCj;
            PodeEditar = desabilitado;
        }

        public long AulaId { get; }
        public string DataAula
        {
            get
            {
                return  AulaCj ? Data.ToString("dd/MM/yyyy") + " Aula CJ" : Data.ToString("dd/MM/yyyy");
            }
        }
        public DateTime Data { get; }
        public int NumeroAulas { get; }
        public long? FrequenciaId { get; }
        public bool EhReposicao { get; }
        public bool AulaCj { get; }
        public bool PodeEditar { get; }
        public bool EhDataSelecionadaFutura => Data.Date > DateTime.Now.Date;
    }
}
