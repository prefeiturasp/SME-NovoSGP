using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EventoDataDto
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public string Nome { get; set; }
        public string TipoEvento { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
    }
}
