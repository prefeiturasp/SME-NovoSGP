using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Conceito
    {
        public long Id { get; set; }
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public bool Aprovado { get; set; }
        public bool Ativo { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
    }
}
