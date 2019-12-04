using System;

namespace SME.SGP.Dominio
{
    public class NotaParametro : EntidadeBase
    {
        public bool Ativo { get; set; }
        public DateTime FimVigencia { get; set; }
        public double Incremento { get; set; }
        public DateTime InicioVigencia { get; set; }
        public double Maxima { get; set; }
        public double Media { get; set; }
        public double Minima { get; set; }
    }
}