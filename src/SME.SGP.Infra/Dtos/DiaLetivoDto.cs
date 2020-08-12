using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DiaLetivoDto
    {
        public DiaLetivoDto()
        {
            UesIds = new List<string>();
        }
        public DateTime Data { get; set; }
        public bool EhLetivo { get; set; }
        public List<string> UesIds { get; set; }
    }
}
