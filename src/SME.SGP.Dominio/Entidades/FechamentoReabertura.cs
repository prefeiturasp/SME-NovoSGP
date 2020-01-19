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
        public DateTime Inicio { get; set; }
        public bool Migrado { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public Ue Ue { get; set; }
        public long UeId { get; set; }
        private List<FechamentoReaberturaBimestre> bimestres { get; set; }

        public void Adicionar(FechamentoReaberturaBimestre bimestre)
        {
            if (bimestre != null)
            {
                bimestre.FechamentoAbertura = this;
                bimestre.FechamentoAberturaId = this.Id;
                bimestres.Add(bimestre);
            }
        }
    }
}