using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class FechamentoReabertura : EntidadeBase
    {
        public FechamentoReabertura()
        {
            bimestres = new List<FechamentoReaberturaBimestre>();
        }

        public IEnumerable<FechamentoReaberturaBimestre> Bimestres { get { return bimestres; } }
        public string Descricao { get; set; }
        public Dre Dre { get; set; }
        public long DreId { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Incio { get; set; }
        public bool Migrado { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public Ue Ue { get; set; }
        public long UeId { get; set; }
        private List<FechamentoReaberturaBimestre> bimestres { get; set; }
    }
}