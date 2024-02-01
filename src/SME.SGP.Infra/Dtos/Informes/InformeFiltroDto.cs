using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Informes
{
    public class InformeFiltroDto
    {
        public long? DreId { get; set; }
        public long? UeId { get; set; }
		public DateTime? DataEnvioInicio { get; set; }
        public DateTime? DataEnvioFim { get; set; }
        public IEnumerable<long> Perfis { get; set; }
        public string Titulo { get; set; }
    }
}
