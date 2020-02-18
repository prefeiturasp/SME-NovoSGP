using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class NotaTipoValor : EntidadeBase
    {
        public TipoNota TipoNota { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
    }
}
