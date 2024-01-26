using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroTurmaRegistrosAcaoDto
    {
        public int AnoLetivo { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public long? TurmaId { get; set; }
        public int? Modalidade { get; set; }
        public int Semestre { get; set; }
    }
}