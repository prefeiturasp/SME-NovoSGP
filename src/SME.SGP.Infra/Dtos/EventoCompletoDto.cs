using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Infra
{
    public class EventoCompletoDto : EventoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public long Id { get; set; }
        public EventoTipoDto TipoEvento { get; set; }
        public bool? PodeAlterar { get; set; }
        public EntidadeStatus Status { get; set; }
    }
}