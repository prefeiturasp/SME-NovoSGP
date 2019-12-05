using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAulaExistenteRetornoDto
    {
        public int TurmaId { get; set; }
        public bool Existe { get; set; }
    }
}
