using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class TipoCalendarioCompletoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public string DescricaoPeriodo { get; set; }
        public long Id { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public string Nome { get; set; }
        public Periodo Periodo { get; set; }
        public bool PossuiEventos { get; set; }
        public bool Situacao { get; set; }
    }
}