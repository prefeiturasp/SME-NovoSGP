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
        public string Nome { get; set; }
        public int QuantidadeDuracaoAula { get; set; }
        public int Semestre { get; set; }
        public int TipoTurno { get; set; }

        public Ue Ue { get; set; }
        public long UeId { get; set; }

        public void AdicionarUe(Ue ue)
        {
            Ue = ue;
        }
    }
}