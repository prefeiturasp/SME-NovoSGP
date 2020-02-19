using System;

namespace SME.SGP.Infra
{
    public class TipoAvaliacaoCompletaDto : TipoAvaliacaoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public long Id { get; set; }
        public bool? possuiAvaliacao { get; set; }
        public int AvaliacoesNecessariasPorBimestre { get; set; }
    }
}