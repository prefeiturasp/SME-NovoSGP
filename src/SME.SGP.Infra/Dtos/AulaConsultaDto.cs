using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaConsultaDto
    {
        public long Id { get; set; }
        public long DisciplinaId { get; set; }
        public long TurmaId { get; set; }
        public long TipoCalendarioId { get; set; }
        public TipoAula TipoAula { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataAula { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
    }
}
