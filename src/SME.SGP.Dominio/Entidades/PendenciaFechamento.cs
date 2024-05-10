using System;
using System.Collections.Generic;
using System.Text;

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

        public FechamentoTurmaDisciplina FechamentoTurmaDisciplina { get; set; }
        public long FechamentoTurmaDisciplinaId { get; set; }
        public Pendencia Pendencia { get; set; }
        public long PendenciaId { get; set; }
    }
}
