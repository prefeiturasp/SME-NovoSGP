using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PersistenciaPlanoAEEObservacaoDto
    {
        public long Id { get; set; }
        public long PlanoAEEId { get; set; }
        public string Observacao { get; set; }
        public IEnumerable<long> Usuarios { get; set; }
    }
}
