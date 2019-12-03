using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class NotaParametro
    {
        public long Id { get; set; }
        public double Minima { get; set; }
        public double Media { get; set; }
        public double Maxima { get; set; }
        public double Incremento { get; set; }
        public bool Ativo { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
    }
}
