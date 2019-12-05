using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class NotaConceitoLista
    {
        public IEnumerable<NotaConceitoDto> NotasConceitos { get; set; }
        public string TurmaId { get; set; }
    }
}
