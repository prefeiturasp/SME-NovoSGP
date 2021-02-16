using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAEEReestruturacaoDto
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public long VersaoId { get; set; }
        public string Versao { get; set; }
        public string Descricao { get; set; }
    }
}
