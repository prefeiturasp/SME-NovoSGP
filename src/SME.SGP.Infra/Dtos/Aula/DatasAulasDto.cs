using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DatasAulasDto
    {
        public DateTime Data { get; set; }
        public IEnumerable<AulaSimplesDto> Aulas { get; set; }
    }
}
