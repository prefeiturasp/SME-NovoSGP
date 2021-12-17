using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoPorAulaDto
    {
        public long AulaId { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int NumeroAula { get; set; }
        public TipoFrequencia TipoFrequencia { get; set; }
    }
}
