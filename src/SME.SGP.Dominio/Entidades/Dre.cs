using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class Dre
    {
        public long Id { get; set; }
        public string  CodigoDre { get; set; }
        public string Abreviacao { get; set; }
        public string Nome { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
