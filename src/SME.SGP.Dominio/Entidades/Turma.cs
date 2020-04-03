using System;

namespace SME.SGP.Dominio
{
    public class Turma
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public long Id { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario 
        { 
            get => ModalidadeCodigo == Modalidade.EJA ? 
                ModalidadeTipoCalendario.EJA : 
                ModalidadeTipoCalendario.FundamentalMedio; 
        }
        public string Nome { get; set; }
        public int QuantidadeDuracaoAula { get; set; }
        public int Semestre { get; set; }
        public int TipoTurno { get; set; }

        public Ue Ue { get; set; }
        public long UeId { get; set; }

        public void AdicionarUe(Ue ue)
        {
            if (ue != null)
            {
                Ue = ue;
                UeId = ue.Id;
            }
        }

        public ModalidadeTipoCalendario ObterModalidadeTipoCalendario()
        {
            if (ModalidadeCodigo == Modalidade.Fundamental || ModalidadeCodigo == Modalidade.Medio)
                return ModalidadeTipoCalendario.FundamentalMedio;
            else return ModalidadeTipoCalendario.EJA;
        }

        public bool MesmaModalidadePeriodoEscolar(ModalidadeTipoCalendario modalidade)
        {
            if (modalidade == ModalidadeTipoCalendario.EJA)
                return ModalidadeCodigo == Modalidade.EJA;
            else
                return ModalidadeCodigo != Modalidade.EJA;
        }
    }
}