using System;

namespace SME.SGP.Infra
{
    public class PlanoAEEVersaoDto
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
    }
}
