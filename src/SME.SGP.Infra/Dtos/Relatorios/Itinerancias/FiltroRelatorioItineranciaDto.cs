using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioItineranciaDto
    {
        public IEnumerable<long> Itinerancias { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }
    }
}
