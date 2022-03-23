using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroImpressaoOcorrenciaDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public long TurmaId { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public IEnumerable<long> OcorrenciasIds { get; set; }
    }
}
