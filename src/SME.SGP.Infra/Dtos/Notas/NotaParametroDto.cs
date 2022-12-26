using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.Notas
{
    public class NotaParametroDto
    {
        public bool Ativo { get; set; }
        public double Incremento { get; set; }
        public double Maxima { get; set; }
        public double Media { get; set; }
        public double Minima { get; set; }

    }
}
