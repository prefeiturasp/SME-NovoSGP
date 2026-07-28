using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaFechamento : EntidadeBase
    {
        public PendenciaFechamento() { }
        public PendenciaFechamento(long fechamentoTurmaDisciplinaId, long pendenciaId)
        {
            this.FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            this.PendenciaId = pendenciaId;
        }

        [Computed]
        public FechamentoTurmaDisciplina FechamentoTurmaDisciplina { get; set; }
        public long FechamentoTurmaDisciplinaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        public long PendenciaId { get; set; }
    }
}
