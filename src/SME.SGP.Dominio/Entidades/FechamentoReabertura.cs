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

        public void AtualizarDre(Dre dre)
        {
            if (dre != null)
            {
                this.Dre = dre;
                this.DreId = dre.Id;
            }
        }

        public void AtualizarTipoCalendario(TipoCalendario tipoCalendario)
        {
            if (tipoCalendario != null)
            {
                this.TipoCalendario = tipoCalendario;
                this.TipoCalendarioId = tipoCalendario.Id;
            }
        }

        public void AtualizarUe(Ue ue)
        {
            if (ue != null)
            {
                this.Ue = ue;
                this.UeId = ue.Id;
            }
        }

        public bool EhParaDre()
        {
            return Ue is null;
        }

        public bool EhParaUe()
        {
            return !(Dre is null) && !(Ue is null);
        }

        public void PodeSalvar()
        {
            if (Inicio > Fim)
                throw new NegocioException("A data início não pode ser maior que a data fim.");

            if (TipoCalendario.AnoLetivo != Inicio.Year || TipoCalendario.AnoLetivo != Fim.Year)
                throw new NegocioException("O ano não pode ser diferente do ano do Tipo de Calendário.");
        }
    }
}