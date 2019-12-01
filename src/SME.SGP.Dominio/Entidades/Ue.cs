using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class Ue
    {
        public long Id { get; set; }
        public string CodigoUe { get; set; }
        public string Nome { get; set; }
        public int TipoEscola { get; set; }

        public long DreId { get; set; }
        public Dre Dre { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
