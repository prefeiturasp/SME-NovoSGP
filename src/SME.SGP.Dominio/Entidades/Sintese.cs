using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Sintese : EntidadeBase
    {
        public bool Aprovado { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public DateTime FimVigencia { get; set; }
        public DateTime InicioVigencia { get; set; }
        public string Valor { get; set; }
    }
}
