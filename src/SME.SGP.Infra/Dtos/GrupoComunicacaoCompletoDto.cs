using System;

namespace SME.SGP.Infra
{
    public class GrupoComunicacaoCompletoDto : GrupoComunicacaoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
    }
}