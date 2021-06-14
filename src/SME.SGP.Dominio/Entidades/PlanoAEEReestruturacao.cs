using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class PlanoAEEReestruturacao : EntidadeBase
    {
        public PlanoAEEVersao PlanoAEEVersao { get; set; }
        public long PlanoAEEVersaoId { get; set; }
        public int Semestre { get; set; }
        public string Descricao { get; set; }
    }
}
