using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class Turma
    {
        public long Id { get; set; }
        public string CodigoTurma { get; set; }
        public string Nome { get; set; }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public int ModalidadeCodigo { get; set; }
        public int Semestre { get; set; }
        public int QuantidadeDuracaoAula { get; set; }
        public int TipoTurno { get; set; }

        public long UeId { get; set; }
        public Ue Ue { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
